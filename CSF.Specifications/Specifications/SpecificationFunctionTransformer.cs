//
// SpecificationFunctionTransformer.cs
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
namespace CSF.Specifications
{
    /// <summary>
    /// A concrete specification transformer class.
    /// </summary>
    class SpecificationFunctionTransformer<TOrigin> : IGetsTransformedSpecificationFunction<TOrigin>
    {
        readonly ISpecificationFunction<TOrigin> spec;

        /// <summary>
        /// Transforms the specification to a target type, using a selector function.
        /// </summary>
        /// <returns>The transformed specification function.</returns>
        /// <param name="selector">A selector to specify how an instance of the transformed/target type may be used to get an instance of the originally-specified type.</param>
        /// <typeparam name="TTarget">The target type.</typeparam>
        public ISpecificationFunction<TTarget> To<TTarget>(Func<TTarget, TOrigin> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var func = spec.GetFunction();
            Func<TTarget, bool> transformed = target => func(selector(target));
            return Spec.Func(transformed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationFunctionTransformer{TOrigin}"/> class.
        /// </summary>
        /// <param name="spec">A specification to transform.</param>
        public SpecificationFunctionTransformer(ISpecificationFunction<TOrigin> spec)
        {
            this.spec = spec ?? throw new ArgumentNullException(nameof(spec));
        }
    }
}
