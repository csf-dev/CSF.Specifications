//
// Predicate.cs
//
// Authors:
//       Pete Montgomery https://petemontgomery.wordpress.com
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2011 Pete Montgomery, further work 2018 Craig Fowler
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
using System.Linq.Expressions;

namespace CSF
{
  /// <summary>
  /// Static helper type which provides functionality to compose and combine predicate expressions.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The original source of this work is https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/
  /// </para>
  /// <para>
  /// Some (largely cosmetic) modifications have been made afterwards.
  /// </para>
  /// </remarks>
  public static class Predicate
  {
    /// <summary>
    /// Gets a predicate expression which always evaluates to true.
    /// </summary>
    public static Expression<Func<T, bool>> True<T>() { return param => true; }

    /// <summary>
    /// Gets a predicate expression which always evaluates to false.
    /// </summary>
    public static Expression<Func<T, bool>> False<T>() { return param => false; }

    /// <summary>
    /// Convenience method to create an expression instance from an expression lambda.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The only real reason for this is so that client code can avoid unwanted explicit typing or casting in order
    /// to create expression instances from shorthand lambdas.  Compare the two following examples:
    /// </para>
    /// <code>
    /// // Explicit type
    /// Expression&lt;Func&lt;MyObject, bool&gt;&gt; expression = obj =&gt; obj.HowMany &gt; 3;
    /// 
    /// // Implied type
    /// var expression = Predicate.Create&lt;MyObject&gt;(obj =&gt; obj.HowMany &gt; 3);
    /// </code>
    /// <para>
    /// The implied type is more suited to intellisense and other autocomplete features of IDEs.
    /// </para>
    /// </remarks>
    /// <returns>The expression instance.</returns>
    /// <param name="expression">An expression, typically a shorthand.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> expression) => expression;

    /// <summary>
    /// Extension method which combines the two expressions using a logical 'AND'.
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
                                                   Expression<Func<T, bool>> second)
    {
      return Compose(first, second, Expression.AndAlso);
    }

    /// <summary>
    /// Extension method which combines the two expressions using a logical 'OR'.
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
                                                  Expression<Func<T, bool>> second)
    {
      return Compose(first, second, Expression.OrElse);
    }

    /// <summary>
    /// Extension method which negates the given predicate; a logical 'NOT'.
    /// </summary>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
      var negated = Expression.Not(expression.Body);
      return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
    }

    /// <summary>
    /// Combines the first expression with the second using the given composition function.
    /// </summary>
    public static Expression<T> Compose<T>(Expression<T> firstExpression,
                                           Expression<T> secondExpression,
                                           Func<Expression, Expression, Expression> compositionFunction)
    {
      var mappedParameters = MapExpressionParameters(firstExpression, secondExpression);
      var bodyOfSecondExpressionWithReplacedParams = ReplaceParameters(mappedParameters, secondExpression.Body);
      return ComposeExpressionBodies(firstExpression,
                                     bodyOfSecondExpressionWithReplacedParams,
                                     compositionFunction);
    }

    static Expression<T> ComposeExpressionBodies<T>(Expression<T> firstExpression,
                                                    Expression secondExpressionBody,
                                                    Func<Expression, Expression, Expression> compositionFunction)
    {
      var composedBody = compositionFunction(firstExpression.Body, secondExpressionBody);
      return Expression.Lambda<T>(composedBody, firstExpression.Parameters);
    }

    static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> replacementParams,
                                        Expression sourceExpression)
    {
      return new ParameterRebinder(replacementParams).Visit(sourceExpression);
    }

    static Dictionary<ParameterExpression, ParameterExpression> MapExpressionParameters<T>(Expression<T> first,
                                                                                           Expression<T> second)
    {
      if(first == null)
        throw new ArgumentNullException(nameof(first));
      if(second == null)
        throw new ArgumentNullException(nameof(second));
      if(first.Parameters.Count != second.Parameters.Count)
        throw new ArgumentException("First and second expressions must have identical parameter counts.");

      return first
        .Parameters
        .Select((firstParam, idx) => new { firstParam, secondParam = second.Parameters[idx] })
        .ToDictionary(k => k.secondParam, v => v.firstParam);
    }
  }
}
