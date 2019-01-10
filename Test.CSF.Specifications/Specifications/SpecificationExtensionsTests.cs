//
// SpecificationExtensionsTests.cs
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
using CSF.Specifications;
using NUnit.Framework;
using Test.CSF.Specifications.Stubs;

namespace Test.CSF.Specifications
{
  [TestFixture,Parallelizable]
  public class SpecificationExtensionsTests
  {
    [Test,AutoMoqData]
    public void And_creates_combined_expression_which_must_satisfy_both(Person personOne,
                                                                        Person personTwo,
                                                                        Person personThree)
    {
      // Arrange
      personOne.Identity = 1;
      personOne.Name = "Bob";
      personTwo.Identity = 2;
      personTwo.Name = "Anna";
      personThree.Identity = 3;
      personThree.Name = "Anna";

      var firstSpec = new PersonNameSpecificationExpression("Anna");
      var secondSpec = new PersonIdentifierSpecificationExpression(2);

      var combinedSpec = firstSpec.And(secondSpec);

      var people = new [] { personOne, personTwo, personThree }.AsQueryable();

      // Act
      var result = combinedSpec.ApplyTo(people).ToArray();

      // Assert
      Assert.That(result, Is.EquivalentTo(new [] { personTwo }));
    }

    [Test,AutoMoqData]
    public void Or_creates_combined_expression_which_must_satisfy_either(Person personOne,
                                                                         Person personTwo,
                                                                         Person personThree)
    {
      // Arrange
      personOne.Identity = 1;
      personOne.Name = "Bob";
      personTwo.Identity = 2;
      personTwo.Name = "Anna";
      personThree.Identity = 3;
      personThree.Name = "Jo";

      var firstSpec = new PersonNameSpecificationExpression("Anna");
      var secondSpec = new PersonIdentifierSpecificationExpression(3);

      var combinedSpec = firstSpec.Or(secondSpec);

      var people = new [] { personOne, personTwo, personThree }.AsQueryable();

      // Act
      var result = combinedSpec.ApplyTo(people).ToArray();

      // Assert
      Assert.That(result, Is.EquivalentTo(new [] { personTwo, personThree }));
    }
  }
}
