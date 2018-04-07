//
// SpecificationExpression.cs
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
using System.Linq;
using System.Linq.Expressions;

namespace CSF.Data.Specifications
{
  /// <summary>
  /// Base type for implementations of <see cref="ISpecificationExpression{T}"/> implementations, which
  /// create a specification based upon a Linq Expression type.
  /// </summary>
  public abstract class SpecificationExpression<T> : Specification<T>, ISpecificationExpression<T>
  {
    /// <summary>
    /// Applies the current specification instance to the given query as a 'where' condition.
    /// </summary>
    /// <returns>The query, modified by the application of the current specification instance.</returns>
    /// <param name="query">The query to which this specification should be applied.</param>
    public override IQueryable<T> ApplyTo(IQueryable<T> query)
    {
      if(query == null)
        throw new ArgumentNullException(nameof(query));

      return query.Where(GetExpression());
    }

    /// <summary>
    /// Gets a value which indicates whether or not the given object matches this specification instance or not.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the object matches this specification; <c>false</c> otherwise.</returns>
    /// <param name="obj">The object instance to test against the specification.</param>
    public override bool Matches(T obj)
    {
      var compiledExpression = GetCompiledExpression();
      return compiledExpression(obj);
    }

    /// <summary>
    /// Gets a <c>Predicate&lt;T&gt;</c> instance which is equivalent to the expression which is encapsulated by
    /// the current specification instance.
    /// </summary>
    /// <returns>A predicate equivalent to the current specification.</returns>
    public virtual Predicate<T> AsPredicate()
    {
      var compiledExpression = GetCompiledExpression();
      return obj => compiledExpression(obj);
    }

    /// <summary>
    /// Gets a specification instance which is equivalent to the logical 'NOT' of the current specification instance.
    /// </summary>
    /// <returns>A specification which is the complement/logical 'NOT' of the current instance.</returns>
    public ISpecificationExpression<T> Negate()
    {
      var negatedExpression = GetExpression().Not();
      return Specification.Create(negatedExpression);
    }

    /// <summary>
    /// Gets the expression which is encapsulated by the current specification instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Override this in your derived implementation to provide the appropriate logic for your specification
    /// class.
    /// </para>
    /// </remarks>
    /// <returns>The expression.</returns>
    public abstract Expression<Func<T, bool>> GetExpression();

    Func<T, bool> GetCompiledExpression() => GetExpression().Compile();
  }
}
