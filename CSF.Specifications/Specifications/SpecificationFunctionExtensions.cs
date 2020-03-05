//
// SpecificationFunctionExtensions.cs
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
    /// Extension methods for specification functions.
    /// </summary>
    public static class SpecificationFunctionExtensions
    {
        /// <summary>
        /// Gets a value which indicates whether a specified value matches/satisfies the specification.
        /// </summary>
        /// <returns><c>true</c> if the value matches the specification; <c>false</c> otherwise.</returns>
        /// <param name="spec">A specification function.</param>
        /// <param name="value">The value to test with the specification.</param>
        /// <typeparam name="T">The generic type of the specification and the specific type of the value object.</typeparam>
        public static bool Matches<T>(this ISpecificationFunction<T> spec, T value)
        {
            var func = GetFunction(spec);
            return func(value);
        }

        /// <summary>
        /// Gets a <c>System.Predicate&lt;T&gt;</c> from a specification function.
        /// </summary>
        /// <returns>A predicate instance matching the specification.</returns>
        /// <param name="spec">A specification function.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static Predicate<T> AsPredicate<T>(this ISpecificationFunction<T> spec)
        {
            var func = GetFunction(spec);
            return o => func(o);
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// negation of the specified object: <c>NOT</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification function.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static ISpecificationFunction<T> Not<T>(this ISpecificationFunction<T> spec)
        {
            var func = GetFunction(spec);
            return Spec.Func<T>(o => !func(o));
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// combination of the specified objects: <c>AND</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification function.</param>
        /// <param name="composeWith">Another specification function.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> And<T>(this ISpecificationFunction<T> spec, ISpecificationFunction<T> composeWith)
        {
            var func1 = GetFunction(spec);
            var func2 = GetFunction(composeWith);
            return Spec.Func<T>(o => func1(o) && func2(o));
        }
        
        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// combination of the specified objects: <c>AND</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification function.</param>
        /// <param name="composeWith">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> And<T>(this ISpecificationFunction<T> spec, ISpecificationExpression<T> composeWith)
        {
            var func1 = GetFunction(spec);
            var func2 = composeWith.GetFunction();
            return Spec.Func<T>(o => func1(o) && func2(o));
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// alternation of the specified objects: <c>OR</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification function.</param>
        /// <param name="composeWith">Another specification function.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> Or<T>(this ISpecificationFunction<T> spec, ISpecificationFunction<T> composeWith)
        {
            var func1 = GetFunction(spec);
            var func2 = GetFunction(composeWith);
            return Spec.Func<T>(o => func1(o) || func2(o));
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// alternation of the specified objects: <c>OR</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification function.</param>
        /// <param name="composeWith">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> Or<T>(this ISpecificationFunction<T> spec, ISpecificationExpression<T> composeWith)
        {
            var func1 = GetFunction(spec);
            var func2 = composeWith.GetFunction();
            return Spec.Func<T>(o => func1(o) || func2(o));
        }

        static Func<T,bool> GetFunction<T>(ISpecificationFunction<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var func = spec.GetFunction();
            if (func == null) throw new ArgumentException($"{nameof(ISpecificationFunction<T>)}<T>.{nameof(ISpecificationFunction<T>.GetFunction)}() must not return null.", nameof(spec));
            return func;
        }
    }
}
