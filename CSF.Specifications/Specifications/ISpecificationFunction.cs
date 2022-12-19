//
// ISpecification`1.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2018 Craig Fowler
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
using System;

namespace CSF.Specifications
{
    /// <summary>
    /// A specification object is a wrapper for a predicate function.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A specification function may contain arbitrary predicate logic to determine
    /// whether or not an object matches that predicate or not.
    /// </para>
    /// <para>
    /// Where possible, prefer using specification expressions - <see cref="ISpecificationFunction{T}"/> -
    /// instead of specification functions.  Specification functions CANNOT BE USED where an
    /// <c>Expression&lt;Func&lt;T,bool&gt;&gt;</c> is required, for example with <c>IQueryable&lt;T&gt;</c>.
    /// </para>
    /// </remarks>
    public interface ISpecificationFunction<T>
    {
        /// <summary>
        /// Gets the predicate function provided by the current specification instance.
        /// </summary>
        /// <returns>A predicate function</returns>
        Func<T, bool> GetFunction();

#if NETCOREAPP3_1_OR_GREATER

        /// <summary>
        /// Operator overload equivalent to <see cref="SpecificationFunctionExtensions.And{T}(ISpecificationFunction{T}, ISpecificationFunction{T})" />.
        /// </summary>
        /// <param name="first">A specification function</param>
        /// <param name="second">A specification function</param>
        /// <returns>A specification function that is the logical AND of the two specified specifications.</returns>
        static ISpecificationFunction<T> operator &(ISpecificationFunction<T> first, ISpecificationFunction<T> second) => first.And(second);

        /// <summary>
        /// Operator overload equivalent to <see cref="SpecificationFunctionExtensions.And{T}(ISpecificationFunction{T}, ISpecificationExpression{T})" />.
        /// </summary>
        /// <param name="first">A specification function</param>
        /// <param name="second">A specification expression</param>
        /// <returns>A specification function that is the logical AND of the two specified specifications.</returns>
        static ISpecificationFunction<T> operator &(ISpecificationFunction<T> first, ISpecificationExpression<T> second) => first.And(second);

        /// <summary>
        /// Operator overload equivalent to <see cref="SpecificationFunctionExtensions.Or{T}(ISpecificationFunction{T}, ISpecificationFunction{T})" />.
        /// </summary>
        /// <param name="first">A specification function</param>
        /// <param name="second">A specification function</param>
        /// <returns>A specification function that is the logical OR of the two specified specifications.</returns>
        static ISpecificationFunction<T> operator |(ISpecificationFunction<T> first, ISpecificationFunction<T> second) => first.Or(second);

        /// <summary>
        /// Operator overload equivalent to <see cref="SpecificationFunctionExtensions.Or{T}(ISpecificationFunction{T}, ISpecificationExpression{T})" />.
        /// </summary>
        /// <param name="first">A specification function</param>
        /// <param name="second">A specification expression</param>
        /// <returns>A specification function that is the logical OR of the two specified specifications.</returns>
        static ISpecificationFunction<T> operator |(ISpecificationFunction<T> first, ISpecificationExpression<T> second) => first.Or(second);

        /// <summary>
        /// Operator overload equivalent to <see cref="SpecificationFunctionExtensions.Not{T}(ISpecificationFunction{T})" />.
        /// </summary>
        /// <param name="spec">A specification function</param>
        /// <returns>A specification function that is the logical NOT of the specified specifications.</returns>
        static ISpecificationFunction<T> operator !(ISpecificationFunction<T> spec) => spec.Not();

#endif
    }
}
