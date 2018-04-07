//
// PredicateBuilder.cs
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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSF.Data
{
  class ParameterRebinder : ExpressionVisitor
  {
    readonly Dictionary<ParameterExpression, ParameterExpression> parametersAndReplacements;

    protected override Expression VisitParameter(ParameterExpression p)
    {
      ParameterExpression replacement;

      if(parametersAndReplacements.TryGetValue(p, out replacement))
      {
        p = replacement;
      }

      return base.VisitParameter(p);
    }

    internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> parametersAndReplacements)
    {
      this.parametersAndReplacements = parametersAndReplacements ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }
  }
}
