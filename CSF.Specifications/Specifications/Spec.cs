//
// Specification.cs
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
    /// Static factory class used to spawn dynamic specification objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Often, creating specification objects this way isn't such a good
    /// idea.  It would be better to encapsulate the logic within a class
    /// which may be reused.
    /// </para>
    /// </remarks>
    public static class Spec
    {
        /// <summary>
        /// Creates a specification function from a predicate function.
        /// </summary>
        /// <returns>The specification function.</returns>
        /// <param name="function">An arbitrary predicate function.</param>
        /// <typeparam name="T">The type of object tested by the predicate.</typeparam>
        public static ISpecificationFunction<T> Func<T>(Func<T, bool> function)
            => new DynamicSpecFunction<T>(function ?? throw new ArgumentNullException(nameof(function)));

        /// <summary>
        /// Creates a dynamic specification expression from a predicate expression.
        /// </summary>
        /// <returns>The specification expression.</returns>
        /// <param name="expression">A predicate expression.</param>
        /// <typeparam name="T">The type of object tested by the predicate.</typeparam>
        public static ISpecificationExpression<T> Expr<T>(Expression<Func<T, bool>> expression)
            => new DynamicSpecExpression<T>(expression ?? throw new ArgumentNullException(nameof(expression)));
    }
}
