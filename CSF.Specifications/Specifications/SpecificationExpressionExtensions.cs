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
using System.Linq.Expressions;

namespace CSF.Specifications
{
    /// <summary>
    /// Extension methods for specification expressions.
    /// </summary>
    public static class SpecificationExpressionExtensions
    {
        /// <summary>
        /// Gets a value which indicates whether a specified value matches/satisfies the specification.
        /// </summary>
        /// <returns><c>true</c> if the value matches the specification; <c>false</c> otherwise.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <param name="value">The value to test with the specification.</param>
        /// <typeparam name="T">The generic type of the specification and the specific type of the value object.</typeparam>
        public static bool Matches<T>(this ISpecificationExpression<T> spec, T value)
        {
            var func = spec.GetFunction();
            return func(value);
        }

        /// <summary>
        /// Gets a <c>System.Predicate&lt;T&gt;</c> from a specification expression.
        /// </summary>
        /// <returns>A predicate instance matching the specification.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static Predicate<T> AsPredicate<T>(this ISpecificationExpression<T> spec)
        {
            var func = spec.GetFunction();
            return o => func(o);
        }

        /// <summary>
        /// Gets a new specification expression which corresponds to the logical
        /// negation of the specified object: <c>NOT</c>.
        /// </summary>
        /// <returns>A specification expression.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static ISpecificationExpression<T> Not<T>(this ISpecificationExpression<T> spec)
        {
            return Spec.Expr(spec.GetExpressionOrThrow().Not());
        }

        /// <summary>
        /// Gets a new specification expression which corresponds to the logical
        /// combination of the specified objects: <c>AND</c>.
        /// </summary>
        /// <returns>A specification expression.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <param name="composeWith">Another specification expression.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationExpression<T> And<T>(this ISpecificationExpression<T> spec, ISpecificationExpression<T> composeWith)
        {
            var func1 = spec.GetExpressionOrThrow();
            var func2 = composeWith.GetExpressionOrThrow();
            return Spec.Expr(func1.And(func2));
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// combination of the specified objects: <c>AND</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <param name="composeWith">A specification function.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> And<T>(this ISpecificationExpression<T> spec, ISpecificationFunction<T> composeWith)
        {
            var func1 = spec.GetFunction();
            var func2 = composeWith.GetFunction();
            return Spec.Func<T>(o => func1(o) && func2(o));
        }

        /// <summary>
        /// Gets a new specification expression which corresponds to the logical
        /// alternation of the specified objects: <c>OR</c>.
        /// </summary>
        /// <returns>A specification expression.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <param name="composeWith">Another specification expression.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationExpression<T> Or<T>(this ISpecificationExpression<T> spec, ISpecificationExpression<T> composeWith)
        {
            var func1 = spec.GetExpressionOrThrow();
            var func2 = composeWith.GetExpressionOrThrow();
            return Spec.Expr(func1.Or(func2));
        }

        /// <summary>
        /// Gets a new specification function which corresponds to the logical
        /// alternation of the specified objects: <c>OR</c>.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <param name="composeWith">A specification function.</param>
        /// <typeparam name="T">The generic type of the specifications.</typeparam>
        public static ISpecificationFunction<T> Or<T>(this ISpecificationExpression<T> spec, ISpecificationFunction<T> composeWith)
        {
            var func1 = spec.GetFunction();
            var func2 = composeWith.GetFunction();
            return Spec.Func<T>(o => func1(o) || func2(o));
        }

        /// <summary>
        /// Gets a specification function which copies the specified specification expression.
        /// </summary>
        /// <returns>A specification function.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static ISpecificationFunction<T> AsSpecificationFunction<T>(this ISpecificationExpression<T> spec)
        {
            return Spec.Func(spec.GetFunction());
        }

        /// <summary>
        /// Gets a function from a specification expression, as if it were a
        /// <see cref="ISpecificationFunction{T}"/>.
        /// </summary>
        /// <returns>A function, representing the compiled expression contained within the specification expression.</returns>
        /// <param name="spec">A specification expression.</param>
        /// <typeparam name="T">The generic type of the specification.</typeparam>
        public static Func<T, bool> GetFunction<T>(this ISpecificationExpression<T> spec)
        {
            var expr = spec.GetExpressionOrThrow();
            return expr.Compile();
        }

        internal static Expression<Func<T,bool>> GetExpressionOrThrow<T>(this ISpecificationExpression<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var expr = spec.GetExpression();
            if (expr == null) throw new ArgumentException($"{nameof(ISpecificationExpression<T>)}<T>.{nameof(ISpecificationExpression<T>.GetExpression)}() must not return null.", nameof(spec));
            return expr;
        }
    }
}
