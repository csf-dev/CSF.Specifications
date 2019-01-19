//
// SpecificationTestBase.cs
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
  public abstract class SpecificationTestBase
  {
    [Test,AutoMoqData]
    public void Matches_returns_true_when_instance_is_match(Person person)
    {
      // Arrange
      person.Name = "Bob";
      var spec = CreatePersonNameSpecification("Bob");

      // Act
      var result = spec.Matches(person);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Matches_returns_false_when_instance_is_not_match(Person person)
    {
      // Arrange
      person.Name = "Anna";
      var spec = CreatePersonNameSpecification("Bob");

      // Act
      var result = spec.Matches(person);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Matches_object_returns_true_when_instance_is_match(Person person)
    {
      // Arrange
      person.Name = "Bob";
      var spec = CreatePersonNameSpecification("Bob");

      // Act
      var result = ((ISpecification) spec).Matches(person);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Matches_object_returns_false_when_instance_is_not_match(Person person)
    {
      // Arrange
      person.Name = "Anna";
      var spec = CreatePersonNameSpecification("Bob");

      // Act
      var result = ((ISpecification) spec).Matches(person);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Matches_object_returns_false_when_instance_is_incorrect_type(Animal animal)
    {
      // Arrange
      animal.Name = "Bob";
      var spec = CreatePersonNameSpecification("Bob");

      // Act
      var result = ((ISpecification) spec).Matches(animal);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void ApplyTo_filters_query_for_only_matches(Person personOne, Person personTwo)
    {
      // Arrange
      personOne.Identity = 1;
      personOne.Name = "Bob";
      personTwo.Identity = 2;
      personTwo.Name = "Anna";

      var spec = CreatePersonNameSpecification("Bob");

      var query = new [] { personOne, personTwo }.AsQueryable();

      // Act
      var result = spec.ApplyTo(query).Select(x => x.Identity).ToArray();

      // Assert
      Assert.That(result, Is.EquivalentTo(new [] { 1 }));
    }

    protected abstract ISpecification<Person> CreatePersonNameSpecification(string name);
  }
}
