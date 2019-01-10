//
// SpecificationExtensions.cs
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
using System.Collections.Generic;
using System.Linq;

namespace CSF.Data.Specifications
{
  /// <summary>
  /// A class which contains a number of extension methods which relate to <see cref="ISpecification{T}"/> and
  /// <see cref="ISpecificationExpression{T}"/>.
  /// </summary>
  public static class SpecificationExtensions
  {
    #region IQueryable extensions

    /// <summary>
    /// Filters a Linq query based upon a specification instance, applying the specification as a condition.
    /// </summary>
    /// <returns>A query which represents the application of the specification.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static IQueryable<T> Where<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
    {
      if(specification == null)
        throw new ArgumentNullException(nameof(specification));

      return specification.ApplyTo(sourceQuery);
    }

    /// <summary>
    /// Gets the first object from the query which matches the specification.
    /// </summary>
    /// <returns>The matched object.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T First<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).First();

    /// <summary>
    /// Gets the first object from the query which matches the specification, or a default object if no instance was
    /// matched.
    /// </summary>
    /// <returns>The matched object or a default instance.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T FirstOrDefault<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).FirstOrDefault();

    /// <summary>
    /// Gets a single object from the query which matches the specification.
    /// </summary>
    /// <returns>The matched object.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T Single<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).Single();

    /// <summary>
    /// Gets a single object from the query which matches the specification, or a default object if no instance was
    /// matched.
    /// </summary>
    /// <returns>The matched object or a default instance.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T SingleOrDefault<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).SingleOrDefault();

    /// <summary>
    /// Gets a value which indicates whether or not any objects in the query match the given specification.
    /// </summary>
    /// <returns><c>true</c> if there are any matches for the specification; <c>false</c> otherwise.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static bool Any<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).Any();

    /// <summary>
    /// Gets a count of the objects within the query which match the specification.
    /// </summary>
    /// <returns>The count of matching objects.</returns>
    /// <param name="sourceQuery">Source query.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static int Count<T>(this IQueryable<T> sourceQuery, ISpecification<T> specification)
      => sourceQuery.Where(specification).Count();

    #endregion

    #region IEnumerable extensions

    /// <summary>
    /// Filters an enumerable based upon a specification instance, applying the specification as a condition.
    /// </summary>
    /// <returns>A collection of items which represents the result of applying the specification.</returns>
    /// <param name="sourceQuery">Source data.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static IEnumerable<T> Where<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
    {
      if(specification == null)
      {
        throw new ArgumentNullException(nameof(specification));
      }
      return sourceQuery.AsQueryable().Where(specification);
    }

    /// <summary>
    /// Gets the first object from the collection which matches the specification.
    /// </summary>
    /// <returns>The matched object.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T First<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().First(specification);

    /// <summary>
    /// Gets the first object from the collection which matches the specification, or a default object if no instance was
    /// matched.
    /// </summary>
    /// <returns>The matched object or a default instance.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T FirstOrDefault<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().FirstOrDefault(specification);

    /// <summary>
    /// Gets a single object from the collection which matches the specification.
    /// </summary>
    /// <returns>The matched object.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T Single<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().Single(specification);

    /// <summary>
    /// Gets a single object from the collection which matches the specification, or a default object if no instance was
    /// matched.
    /// </summary>
    /// <returns>The matched object or a default instance.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static T SingleOrDefault<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().SingleOrDefault(specification);

    /// <summary>
    /// Gets a value which indicates whether or not any objects in the collection match the given specification.
    /// </summary>
    /// <returns><c>true</c> if there are any matches for the specification; <c>false</c> otherwise.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static bool Any<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().Any(specification);

    /// <summary>
    /// Gets a count of the objects within the collection which match the specification.
    /// </summary>
    /// <returns>The count of matching objects.</returns>
    /// <param name="sourceQuery">Source collection.</param>
    /// <param name="specification">Specification.</param>
    /// <typeparam name="T">The queried object type.</typeparam>
    public static int Count<T>(this IEnumerable<T> sourceQuery, ISpecification<T> specification)
        => sourceQuery.AsQueryable().Count(specification);

    #endregion

    #region composing specifications

    /// <summary>
    /// Creates a new specification which represents the logical 'AND' of the two specifications.
    /// </summary>
    /// <returns>The and.</returns>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    /// <typeparam name="T">The type to which the specifications apply.</typeparam>
    public static ISpecificationExpression<T> And<T>(this ISpecificationExpression<T> first,
                                                 ISpecificationExpression<T> second)
    {
      if(first == null)
        throw new ArgumentNullException(nameof(first));
      if(second == null)
        throw new ArgumentNullException(nameof(second));

      var firstExpression = first.GetExpression();
      var secondExpression = second.GetExpression();

      var combinedExpression = firstExpression.And(secondExpression);
      return Specification.Create(combinedExpression);
    }

    /// <summary>
    /// Creates a new specification which represents the logical 'OR' of the two specifications.
    /// </summary>
    /// <returns>The or.</returns>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    /// <typeparam name="T">The type to which the specifications apply.</typeparam>
    public static ISpecificationExpression<T> Or<T>(this ISpecificationExpression<T> first,
                                                     ISpecificationExpression<T> second)
    {
      if(first == null)
        throw new ArgumentNullException(nameof(first));
      if(second == null)
        throw new ArgumentNullException(nameof(second));

      var firstExpression = first.GetExpression();
      var secondExpression = second.GetExpression();

      var combinedExpression = firstExpression.Or(secondExpression);
      return Specification.Create(combinedExpression);
    }

    #endregion
  }
}
