//
// IPredicateExpression.cs
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
  /// Specialisation of an <see cref="ISpecification{T}" /> which uses a Linq predicate expression as its underlying
  /// source of logic.
  /// </summary>
  public interface ISpecificationExpression<T> : ISpecification<T>
  {
    /// <summary>
    /// Gets the expression which is encapsulated by the current specification instance.
    /// </summary>
    /// <returns>The expression.</returns>
    Expression<Func<T,bool>> GetExpression();

    /// <summary>
    /// Gets a <c>Predicate&lt;T&gt;</c> instance which is equivalent to the expression which is encapsulated by
    /// the current specification instance.
    /// </summary>
    /// <returns>A predicate equivalent to the current specification.</returns>
    Predicate<T> AsPredicate();

    /// <summary>
    /// Gets a specification instance which is equivalent to the logical 'NOT' of the current specification instance.
    /// </summary>
    /// <returns>A specification which is the complement/logical 'NOT' of the current instance.</returns>
    ISpecificationExpression<T> Negate();
  }
}
