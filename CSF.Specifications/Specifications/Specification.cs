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
using System.Linq.Expressions;

namespace CSF.Specifications
{
  /// <summary>
  /// Helper type for the creation of dynamic specifications from expressions.
  /// </summary>
  public static class Specification
  {
    /// <summary>
    /// Creates a new dynamic specification from a given expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is intended for use when creating specifications dynamically, such as when composing/combining other
    /// expressions.  It is not designed for use as the main mechanism of creating first-class specifications.
    /// For that purpose, create an explicit specification implementation which encapsulates your logic within a class.
    /// </para>
    /// </remarks>
    /// <returns>A specification instance.</returns>
    /// <param name="expression">The expression which represents the specification.</param>
    /// <typeparam name="T">The type of object to which the specification applies.</typeparam>
    public static ISpecificationExpression<T> Create<T>(Expression<Func<T,bool>> expression)
    {
      return new DynamicSpecificationExpression<T>(expression);
    }
  }
}
