// The MIT License (MIT)
// 
// Copyright (c) 2014-2015, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.Compiler.Normalization
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using Microsoft.CodeAnalysis.Editing;
	using Modeling;
	using Roslyn.Symbols;
	using Roslyn.Syntax;
	using Utilities;

	/// <summary>
	///   Normalizes classes marked with <see cref="FaultEffectAttribute" />.
	/// </summary>
	public sealed class FaultEffectNormalizer : SyntaxNormalizer
	{
		private readonly Dictionary<Tuple<IMethodSymbol, int>, string> _faultChoiceFields = new Dictionary<Tuple<IMethodSymbol, int>, string>();
		private readonly Dictionary<INamedTypeSymbol, INamedTypeSymbol[]> _faults = new Dictionary<INamedTypeSymbol, INamedTypeSymbol[]>();
		private readonly Dictionary<string, IMethodSymbol> _methodLookup = new Dictionary<string, IMethodSymbol>();
		private readonly Dictionary<string, INamedTypeSymbol> _typeLookup = new Dictionary<string, INamedTypeSymbol>();

		/// <summary>
		///   Normalizes the syntax trees of the <see cref="Compilation" />.
		/// </summary>
		protected override Compilation Normalize()
		{
			var types = Compilation.GetSymbolsWithName(_ => true, SymbolFilter.Type).OfType<INamedTypeSymbol>().ToArray();
			var components = types.Where(type => type.IsComponent(Compilation)).ToArray();
			var faultEffects = types.Where(type => type.IsFaultEffect(Compilation)).ToArray();

			foreach (var type in components.Concat(faultEffects))
				_typeLookup.Add(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), type);

			foreach (var component in components)
			{
				var faults = faultEffects.Where(faultEffect => faultEffect.IsDerivedFrom(component)).ToArray();
				var faultTypes = faults.OrderBy(type => type.GetPriority(Compilation)).ThenBy(type => type.Name).ToArray();
				var nondeterministicFaults = faults.GroupBy(fault => fault.GetPriority(Compilation)).Where(group => group.Count() > 1).ToArray();

				_faults.Add(component, faultTypes);
				foreach (var method in component.GetMembers().OfType<IMethodSymbol>())
				{
					_methodLookup.Add(method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), method);

					foreach (var group in nondeterministicFaults)
					{
						var isNondeterministic = group.Count(f => f.GetMembers().OfType<IMethodSymbol>().Any(m => m.Overrides(method))) > 1;
						if (!isNondeterministic)
							continue;

						var key = Tuple.Create(method, group.Key);
						var fieldName = Guid.NewGuid().ToString().Replace("-", "_").ToSynthesized();

						_faultChoiceFields.Add(key, fieldName);
						AddFaultChoiceField(method.ContainingType, fieldName);
					}
				}

				AddRuntimeTypeField(component);
			}

			return base.Normalize();
		}

		/// <summary>
		///   Normalizes the <paramref name="declaration" />.
		/// </summary>
		public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax declaration)
		{
			var classSymbol = declaration.GetTypeSymbol(SemanticModel);
			if (!classSymbol.IsFaultEffect(SemanticModel))
				return base.VisitClassDeclaration(declaration);

			AddFaultField(classSymbol);

			declaration = (ClassDeclarationSyntax)base.VisitClassDeclaration(declaration);
			return ChangeBaseType(classSymbol, declaration);
		}

		/// <summary>
		///   Normalizes the <paramref name="declaration" />.
		/// </summary>
		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax declaration)
		{
			var methodSymbol = declaration.GetMethodSymbol(SemanticModel);
			if (!methodSymbol.ContainingType.IsFaultEffect(SemanticModel) || !methodSymbol.IsOverride)
				return declaration;

			var memberAccess = Syntax.MemberAccessExpression(Syntax.BaseExpression(), methodSymbol.Name);
			var invocation = Syntax.InvocationExpression(memberAccess, CreateInvocationArguments(methodSymbol));

			declaration = declaration.WithBody(CreateBody(methodSymbol, declaration.Body, invocation));
			return declaration;
		}

		/// <summary>
		///   Normalizes the <paramref name="accessor" />.
		/// </summary>
		public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax accessor)
		{
			var methodSymbol = accessor.GetMethodSymbol(SemanticModel);
			if (!methodSymbol.ContainingType.IsFaultEffect(SemanticModel) || !methodSymbol.IsOverride)
				return accessor;

			var baseExpression = Syntax.MemberAccessExpression(Syntax.BaseExpression(), methodSymbol.GetPropertySymbol().Name);
			if (accessor.Kind() != SyntaxKind.GetAccessorDeclaration)
				baseExpression = Syntax.AssignmentStatement(baseExpression, Syntax.IdentifierName("value"));

			accessor = accessor.WithBody(CreateBody(methodSymbol, accessor.Body, baseExpression));
			return accessor;
		}

		/// <summary>
		///   Creates the arguments for a delegate invocation.
		/// </summary>
		private static IEnumerable<ArgumentSyntax> CreateInvocationArguments(IMethodSymbol methodSymbol)
		{
			return methodSymbol.Parameters.Select(parameter =>
			{
				var argument = SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parameter.Name));

				switch (parameter.RefKind)
				{
					case RefKind.Ref:
						return argument.WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.RefKeyword));
					case RefKind.Out:
						return argument.WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword));
					case RefKind.None:
						return argument;
					default:
						Assert.NotReached("Unsupported ref kind.");
						return null;
				}
			});
		}

		/// <summary>
		///   Creates a deterministic or nondeterministic fault effect body.
		/// </summary>
		private BlockSyntax CreateBody(IMethodSymbol method, StatementSyntax originalBody, SyntaxNode baseEffect)
		{
			originalBody = originalBody.AppendLineDirective(-1).PrependLineDirective(originalBody.GetLineNumber());

			var componentType = _typeLookup[method.ContainingType.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)];
			var faultEffectType = _typeLookup[method.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)];
			var faults = _faults[componentType];
			var baseStatement = !method.ReturnsVoid
				? new[] { Syntax.ReturnStatement(baseEffect) }
				: new[] { Syntax.ExpressionStatement(baseEffect), Syntax.ReturnStatement() };

			IMethodSymbol methodSymbol;
			BlockSyntax body = null;

			if (_methodLookup.TryGetValue(method.OverriddenMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), out methodSymbol))
			{
				var priorityFaults = faults.Where(fault => fault.GetPriority(Compilation) == method.ContainingType.GetPriority(Compilation)).ToArray();
				var overridingEffects = priorityFaults.Where(f => f.GetMembers().OfType<IMethodSymbol>().Any(m => m.Overrides(methodSymbol))).ToArray();
				var overrideCount = overridingEffects.Length;

				if (overrideCount > 1)
				{
					var fieldName = _faultChoiceFields[Tuple.Create(methodSymbol, priorityFaults[0].GetPriority(Compilation))];
					var effectIndex = Array.IndexOf(priorityFaults, faultEffectType);
					var choiceField = Syntax.MemberAccessExpression(Syntax.ThisExpression(), fieldName);

					var levelCondition = Syntax.ValueNotEqualsExpression(choiceField, Syntax.LiteralExpression(effectIndex));
					var ifStatement = Syntax.IfStatement(levelCondition, baseStatement).NormalizeWhitespace().WithTrailingNewLines(1);

					if (overridingEffects.Last().Equals(faultEffectType))
					{
						var levelChoiceVariable = "levelChoice".ToSynthesized();
						var levelCountVariable = "levelCount".ToSynthesized();

						var writer = new CodeWriter();
						writer.AppendLine("unsafe");
						writer.AppendBlockStatement(() =>
						{
							writer.AppendLine($"var {levelChoiceVariable} = stackalloc int[{overrideCount}];");
							writer.AppendLine($"var {levelCountVariable} = 0;");

							for (var i = 0; i < overrideCount; ++i)
							{
								var effectType = overridingEffects[i].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
								var index = Array.IndexOf(priorityFaults, overridingEffects[i]);

								writer.AppendLine($"if ((({effectType})this).{"fault".ToSynthesized()}.IsOccurring)");
								writer.IncreaseIndent();
								writer.AppendLine($"{levelChoiceVariable}[{levelCountVariable}++] = {index};");
								writer.DecreaseIndent();
								writer.NewLine();
							}

							writer.AppendLine($"{fieldName} = {levelCountVariable} == 0 ? - 1 : {levelChoiceVariable}[ChooseIndex({levelCountVariable})];");
						});

						var selectionStatement = SyntaxFactory.ParseStatement(writer.ToString());
						body = SyntaxFactory.Block(selectionStatement, (StatementSyntax)ifStatement, originalBody);
					}
					else
						body = SyntaxFactory.Block((StatementSyntax)ifStatement, originalBody);
				}
			}

			if (body == null)
			{
				var faultAccess = Syntax.MemberAccessExpression(Syntax.ThisExpression(), "fault".ToSynthesized());
				var isOccurring = Syntax.MemberAccessExpression(faultAccess, nameof(Fault.IsOccurring));
				var notOccurring = Syntax.LogicalNotExpression(isOccurring);

				var ifStatement = Syntax.IfStatement(notOccurring, baseStatement).NormalizeWhitespace().WithTrailingNewLines(1);
				body = SyntaxFactory.Block((StatementSyntax)ifStatement, originalBody);
			}

			return body.PrependLineDirective(-1);
		}

		/// <summary>
		///   Changes the base type of the fault effect declaration based on its location in the fault effect list.
		/// </summary>
		private ClassDeclarationSyntax ChangeBaseType(INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDeclaration)
		{
			var baseTypeName = classSymbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var faultEffectSymbol = _typeLookup[classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)];
			var faultIndex = Array.IndexOf(_faults[_typeLookup[baseTypeName]], faultEffectSymbol);
			if (faultIndex == 0)
				return classDeclaration;

			var baseType = Syntax.TypeExpression(_faults[_typeLookup[baseTypeName]][faultIndex - 1]).WithTrivia(classDeclaration.BaseList.Types[0]);
			var baseTypes = SyntaxFactory.SingletonSeparatedList((BaseTypeSyntax)SyntaxFactory.SimpleBaseType((TypeSyntax)baseType));
			var baseList = SyntaxFactory.BaseList(classDeclaration.BaseList.ColonToken, baseTypes).WithTrivia(classDeclaration.BaseList);
			return classDeclaration.WithBaseList(baseList);
		}

		/// <summary>
		///   Adds the runtime type field to the component symbol.
		/// </summary>
		private void AddRuntimeTypeField(INamedTypeSymbol classSymbol)
		{
			var className = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var faults = _faults[_typeLookup[className]];
			var runtimeType = faults.Length > 0 ? faults[faults.Length - 1] : classSymbol;
			var typeofExpression = SyntaxFactory.TypeOfExpression((TypeSyntax)Syntax.TypeExpression((runtimeType)));
			var field = Syntax.FieldDeclaration(
				name: "runtimeType".ToSynthesized(),
				type: SyntaxFactory.ParseTypeName("System.Type"),
				accessibility: Accessibility.Private,
				modifiers: DeclarationModifiers.Static | DeclarationModifiers.ReadOnly,
				initializer: typeofExpression);

			field = Syntax.MarkAsNonDebuggerBrowsable(field);
			field = Syntax.AddAttribute<HiddenAttribute>(field);
			field = field.NormalizeWhitespace();

			AddMembers(classSymbol, new UsingDirectiveSyntax[0], (MemberDeclarationSyntax)field);
		}

		/// <summary>
		///   Adds the fault choice field to the component symbol.
		/// </summary>
		private void AddFaultChoiceField(INamedTypeSymbol classSymbol, string fieldName)
		{
			var field = Syntax.FieldDeclaration(
				name: fieldName,
				type: Syntax.TypeExpression(SpecialType.System_Int32),
				accessibility: Accessibility.Internal);

			field = Syntax.MarkAsNonDebuggerBrowsable(field);
			field = Syntax.AddAttribute<NonSerializableAttribute>(field);
			field = field.NormalizeWhitespace();

			AddMembers(classSymbol, new UsingDirectiveSyntax[0], (MemberDeclarationSyntax)field);
		}

		/// <summary>
		///   Adds the fault field to the fault effect symbol.
		/// </summary>
		private void AddFaultField(INamedTypeSymbol classSymbol)
		{
			var faultField = Syntax.FieldDeclaration(
				name: "fault".ToSynthesized(),
				type: Syntax.TypeExpression<Fault>(SemanticModel),
				accessibility: Accessibility.Internal);

			faultField = Syntax.MarkAsNonDebuggerBrowsable(faultField);
			faultField = Syntax.AddAttribute<HiddenAttribute>(faultField);
			faultField = faultField.NormalizeWhitespace();

			AddMembers(classSymbol, (MemberDeclarationSyntax)faultField);
		}
	}
}