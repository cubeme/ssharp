using NUnit.Framework;
using SafetySharp.Analysis;
using SafetySharp.Modeling;

class FirstSafetySharpModel
{
    [Test]
    public void Test()
    {
        var result = ModelChecker.CheckInvariant(new Model(), true);
        Assert.IsTrue(result.FormulaHolds);
    }
}

class Model : ModelBase
{
    [Root(Role.Environment)]
    public C C = new C();
}

class C : Component
{
}