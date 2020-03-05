//
// QueryableExtensions.cs
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
using System.Linq;

namespace CSF.Specifications
{
    /// <summary>
    /// Extension methods for <c>IQueryable&lt;T&gt;</c>, using specification expressions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Filters a Linq queryable object using the predicate defined in a specification expression.
        /// </summary>
        /// <returns>A queryable object which is filtered by the specification predicate.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            return query.Where(specification.GetExpression());
        }

        /// <summary>
        /// Gets the first object from the query which matches the specification.
        /// </summary>
        /// <returns>The matched object.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static T First<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).First();

        /// <summary>
        /// Gets the first object from the query which matches the specification, or a default object if no instance was
        /// matched.
        /// </summary>
        /// <returns>The matched object or a default instance.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static T FirstOrDefault<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).FirstOrDefault();

        /// <summary>
        /// Gets a single object from the query which matches the specification.
        /// </summary>
        /// <returns>The matched object.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static T Single<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).Single();

        /// <summary>
        /// Gets a single object from the query which matches the specification, or a default object if no instance was
        /// matched.
        /// </summary>
        /// <returns>The matched object or a default instance.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static T SingleOrDefault<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).SingleOrDefault();

        /// <summary>
        /// Gets a value which indicates whether or not any objects in the query match the given specification.
        /// </summary>
        /// <returns><c>true</c> if there are any matches for the specification; <c>false</c> otherwise.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static bool Any<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).Any();

        /// <summary>
        /// Gets a count of the objects within the query which match the specification.
        /// </summary>
        /// <returns>The count of matching objects.</returns>
        /// <param name="query">A Linq queryable object.</param>
        /// <param name="specification">A specification expression.</param>
        /// <typeparam name="T">The generic type of the query and the specification.</typeparam>
        public static int Count<T>(this IQueryable<T> query, ISpecificationExpression<T> specification)
            => query.Where(specification).Count();
    }
}
