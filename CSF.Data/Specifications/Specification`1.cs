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
using System.Linq;

namespace CSF.Data.Specifications
{
  /// <summary>
  /// Base type for implementations of the generic <see cref="Specification{T}"/> type.
  /// </summary>
  public abstract class Specification<T> : ISpecification<T>, ISpecification
  {
    /// <summary>
    /// Applies the current specification instance to the given query as a 'where' condition.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Override this in your own implementation to filter the given query by the logic of the current specification.
    /// </para>
    /// </remarks>
    /// <returns>The query, modified by the application of the current specification instance.</returns>
    /// <param name="query">The query to which this specification should be applied.</param>
    public abstract IQueryable<T> ApplyTo(IQueryable<T> query);

    /// <summary>
    /// Gets a value which indicates whether or not the given object matches this specification instance or not.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the object matches this specification; <c>false</c> otherwise.</returns>
    /// <param name="obj">The object instance to test against the specification.</param>
    public virtual bool Matches(T obj)
    {
      var queryable = new [] { obj }.AsQueryable();
      return ApplyTo(queryable).Any();
    }

    /// <summary>
    /// Gets a value which indicates whether or not the given object matches this specification instance or not.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the object matches this specification; <c>false</c> otherwise.</returns>
    /// <param name="obj">The object instance to test against the specification.</param>
    protected virtual bool Matches(object obj)
    {
      if(!(obj is T)) return false;
      return Matches((T) obj);
    }

    bool ISpecification.Matches(object obj) => Matches(obj);
  }
}
