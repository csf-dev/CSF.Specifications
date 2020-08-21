//
// ExpressionPredicateTransformer.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2020 Craig Fowler
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

namespace CSF
{
    /// <summary>
    /// A class which transforms a Linq predicate expression (such as would be exposed by a specification),
    /// to compose it with a selector in order to change its type.
    /// </summary>
    class ExpressionPredicateTransformer
    {
        /// <summary>
        /// Transforms the specified predicate expression, adding a selector, in order to change the type of the
        /// predicate.
        /// </summary>
        /// <returns>The transformed predicate expression.</returns>
        /// <param name="originalPredicate">The original predicate expression.</param>
        /// <param name="selector">A selector expression.</param>
        /// <typeparam name="TOrigin">The type of the original predicate parameter.</typeparam>
        /// <typeparam name="TTarget">The intended type of the transformed predicate parameter.</typeparam>
        internal Expression<Func<TTarget,bool>> Transform<TOrigin,TTarget>(Expression<Func<TOrigin,bool>> originalPredicate,
                                                                         Expression<Func<TTarget,TOrigin>> selector)
        {
            if (originalPredicate == null)
                throw new ArgumentNullException(nameof(originalPredicate));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var parameterExpr = Expression.Parameter(typeof(TTarget), "target");
            var selectorExpr = Expression.Invoke(selector, new[] { parameterExpr });
            var predicateExpr = Expression.Invoke(originalPredicate, new[] { selectorExpr });

            return Expression.Lambda<Func<TTarget, bool>>(predicateExpr, new[] { parameterExpr });
        }
    }
}
