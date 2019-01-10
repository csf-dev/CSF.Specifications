//
// SpecificationExpressionTests.cs
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
using CSF.Specifications;
using NUnit.Framework;
using Test.CSF.Specifications.Stubs;

namespace Test.CSF.Specifications
{
  [TestFixture,Parallelizable]
  public class SpecificationExpressionTests : SpecificationTestBase
  {
    [Test,AutoMoqData]
    public void AsPredicate_returns_predicate_which_matches_a_matching_object(Person person)
    {
      // Arrange
      person.Name = "Bob";
      var spec = CreatePersonNameSpecificationExpression("Bob");

      // Act
      var result = spec.AsPredicate()(person);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void AsPredicate_returns_predicate_which_does_not_match_a_non_matching_object(Person person)
    {
      // Arrange
      person.Name = "Anna";
      var spec = CreatePersonNameSpecificationExpression("Bob");

      // Act
      var result = spec.AsPredicate()(person);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Negate_returns_specification_which_matches_a_non_matching_object(Person person)
    {
      // Arrange
      person.Name = "Anna";
      var spec = CreatePersonNameSpecificationExpression("Bob");

      // Act
      var result = spec.Negate().Matches(person);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Negate_returns_specification_which_does_not_match_a_matching_object(Person person)
    {
      // Arrange
      person.Name = "Bob";
      var spec = CreatePersonNameSpecificationExpression("Bob");

      // Act
      var result = spec.Negate().Matches(person);

      // Assert
      Assert.That(result, Is.False);
    }

    protected override ISpecification<Person> CreatePersonNameSpecification(string name)
		{
      return CreatePersonNameSpecificationExpression(name);
		}

    ISpecificationExpression<Person> CreatePersonNameSpecificationExpression(string name)
    {
      return new PersonNameSpecificationExpression(name);
    }
	}
}
