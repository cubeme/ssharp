// Copyright (c) 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 Formal Methods and Tools, University of Twente
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
// 
//  * Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 
//  * Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 
//  * Neither the name of the University of Twente nor the names of its
//    contributors may be used to endorse or promote products derived from
//    this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

typedef struct bitvector {
	size_t              n_bits;
	size_t              n_words;

	size_t             *data;
} bitvector_t;

/**
* bitvector_create
*  Create a new bitvector
*   1) unused bitvector_t struct
*   2) number of bits in the vector
*
*  return values:
*   0: n-bit bitvector has been allocated
*  -1: error
*/
extern int          bitvector_create(bitvector_t *, size_t);

/**
* bitvector_clear
*  Set all bits in the bitvector to 0
*   1) the bitvector to clear
*
*  result:
*   All bits in bv are set to 0
*/
extern void         bitvector_clear(bitvector_t *bv);


/**
* bitvector_free
*  Free memory used by a bitvector
*   1) the bitvector to free
*
*  result:
*     frees the memory used by the bitvector
*/
extern void         bitvector_free(bitvector_t *);

/**
* bitvector_copy
*  Copy a bitvector to another bitvector
*   1) unused bitvector_t struct
*   2) source bitvector
*
*  return value:
*   0: new bitvector is created in the target, all bits are copied
*  -1: error
*/
extern int          bitvector_copy(bitvector_t *, const bitvector_t *);

/**
* bitvector_size
*  Returns the size of the bitvector
*   1) the bitvector to get the size from
*
*  return value:
*   the size of the bitvector
*/
extern size_t       bitvector_size(const bitvector_t *);

/**
* bitvector_isset_or_set
*  Set a bit in the bitvector to one return 1 if it was set
*   1) the bitvector to set the bit in
*   2) the index of the bit that must be set to one
*
*  result:
*   the bit in the bitvector is set to one & the previous value is returned
*/
extern int          bitvector_isset_or_set(bitvector_t *bv, size_t idx);

/**
* bitvector_set2
*  Sets two consecutive and 2-aligned bit in the bitvector to a value
*
*   1) the bitvector to test the bits in
*   2) the 2-aligned index idx of the bit that must be set/tested
*   3) the value v to test/set
*
*  result:
*   the bits at idx in the bitvector are set to v
*/
extern void         bitvector_set2(bitvector_t *bv, size_t idx, size_t v);

/**
* bitvector_isset_or_set2
*  Test two consecutive and 2-aligned bit in the bitvector for a value and
*  return 1 if the bitvector was modified
*   1) the bitvector to test the bits in
*   2) the 2-aligned index of the bit that must be set/tested
*   3) the value to test/set
*
*  result:
*   true:  the bits equal v
*   false: the bits not equal v
*/
extern int          bitvector_isset_or_set2(bitvector_t *bv, size_t idx, size_t v);

/**
* bitvector_get
*  Return two consecutive and 2-aligned bits in the bitvector
*   1) the bitvector to test the bits in
*   2) the 2-aligned index of the bit to return
*
*  result:
*   the two bits
*/
extern int          bitvector_get2(const bitvector_t *, size_t);

/**
* bitvector_set
*  Set a bit in the bitvector to one
*   1) the bitvector to set the bit in
*   2) the index of the bit that must be set to one
*
*/
extern void         bitvector_set(bitvector_t *, size_t);
extern void         bitvector_set_atomic(bitvector_t *, size_t);

/**
* bitvector_unset
*  Set a bit in the bitvector to clear
*   1) the bitvector to clear the bit in
*   2) the index of the bit that must be cleared
*
*  result:
*   the bit in the bitvector is cleared
*/
extern void         bitvector_unset(bitvector_t *, size_t);

/**
* bitvector_is_set
*  Test a bit in the bitvector for value one
*   1) the bitvector to test the bit in
*   2) the index of the bit that must be tested
*
*  result:
*   true:  the bit is one
*   false: the bit is not one
*/
extern int          bitvector_is_set(const bitvector_t *, size_t);

/**
* bitvector_union
*  Union two bitvectors
*   1) the target bitvector
*      the bitvector should be created with bitvector_create and
*      should be of the same size as the source bitvector
*   2) the source bitvector
*
*  result:
*   the target bitvector has all bits set that were set in either of the bitvectors
*/
extern void         bitvector_union(bitvector_t *, const bitvector_t *);

/**
* bitvector_intersect
*  Intersect two bitvectors
*   1) the target bitvector
*      the bitvector should be created with bitvector_create or bitvector_copy
*      and should be of the same size as the source bitvector
*   2) the source bitvector
*
*  result:
*   the target bitvector has only those bits set that were set in both vectors
*/
extern void         bitvector_intersect(bitvector_t *, const bitvector_t *);

/**
* bitvector_is_empty
*  Test whether all bits in the bitvector are not set
*   1) the bitvector to test
*
*  result:
*   true:  all bits in the bitvector are not set
*   false: at least one bit in the bitvector is set
*/
extern int          bitvector_is_empty(const bitvector_t *);

/**
* bitvector_is_disjoint
*  Test whether two bitvectors are disjoint or not
*  Disjoint means that the intersection of the two bitvectors is empty
*   1,2) the bitvectors (of the same size) to test
*
*  result:
*   true:  the bitvectors are disjoint
*   false: the bitvectors are not disjoint or not of the same size
*/
extern int          bitvector_is_disjoint(const bitvector_t *, const bitvector_t *);

/**
* bitvector_invert
*  Invert all bits in the bitvector
*   1) the bitvector to invert
*
*  result:
*   All bits that were not set are now set and vice versa
*/
extern void         bitvector_invert(bitvector_t *);