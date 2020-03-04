//
// IPredicateExpression.cs
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
using System.Linq.Expressions;

namespace CSF.Specifications
{
    /// <summary>
    /// A specification object is a wrapper for a predicate expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A specification expression contains predicate logic which determines
    /// whether or not an object matches that predicate or not. A specification
    /// expression is limited only to logic which may be represented via a
    /// <c>System.Linq.Expressions.Expression</c>, and may not include completely
    /// arbitrary logic.
    /// </para>
    /// <para>
    /// Where possible, prefer using specification expressions.  Specification
    /// expression may be used everywhere where specification functions -
    /// <see cref="ISpecificationFunction{T}"/> - can be used, but ALSO in places
    /// where an <c>Expression&lt;Func&lt;T,bool&gt;&gt;</c> is required.  This
    /// includes using them with <c>IQueryable&lt;T&gt;</c>.
    /// </para>
    /// </remarks>
    public interface ISpecificationExpression<T>
    {
        /// <summary>
        /// Gets the predicate function provided by the current specification instance.
        /// </summary>
        /// <returns>A predicate function</returns>
        Expression<Func<T, bool>> GetExpression();
    }
}
