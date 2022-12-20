﻿//
// SpecificationExpressionTransformer.cs
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

namespace CSF.Specifications
{
    /// <summary>
    /// A concrete specification transformer class.
    /// </summary>
    class SpecificationExpressionTransformer<TOrigin> : IGetsTransformedSpecificationExpression<TOrigin>
    {
        readonly ExpressionPredicateTransformer expressionTransformer = new ExpressionPredicateTransformer();
        readonly ISpecificationExpression<TOrigin> spec;

        /// <summary>
        /// Transforms the specification to a target type, using a selector expression.
        /// </summary>
        /// <returns>The transformed specification expression.</returns>
        /// <param name="selector">A selector to specify how an instance of the transformed/target type may be used to get an instance of the originally-specified type.</param>
        /// <typeparam name="TTarget">The target type.</typeparam>
        public ISpecificationExpression<TTarget> To<TTarget>(Expression<Func<TTarget, TOrigin>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var newExpression = expressionTransformer.Transform(spec.GetExpression(), selector);
            return Spec.Expr(newExpression);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationExpressionTransformer{TOrigin}"/> class.
        /// </summary>
        /// <param name="spec">A specification to transform.</param>
        public SpecificationExpressionTransformer(ISpecificationExpression<TOrigin> spec)
        {
            this.spec = spec ?? throw new ArgumentNullException(nameof(spec));
        }
    }
}