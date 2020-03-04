//
// PredicateBuilderTests.cs
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
using CSF;
using NUnit.Framework;
using CSF.Specifications.Tests.Stubs;

namespace CSF.Specifications.Tests
{
  [TestFixture,Parallelizable]
  public class PredicateTests
  {
    [Test,AutoMoqData]
    public void And_creates_predicate_which_satisfies_both_predicates(Person personOne,
                                                                      Person personTwo,
                                                                      Person personThree)
    {
      // Arrange
      personOne.Name = "Billy";
      personTwo.Name = "Bob";
      personThree.Name = "Thornton";

      var firstPredicate = Predicate.Create<Person>(p => p.Name.StartsWith("B", StringComparison.InvariantCulture));
      var secondPredicate = Predicate.Create<Person>(p => p.Name.EndsWith("b", StringComparison.InvariantCulture));

      // Act
      var composedPredicate = firstPredicate.And(secondPredicate);

      // Assert
      Assert.That(() => composedPredicate.Compile()(personOne), Is.False, "First person is not matched");
      Assert.That(() => composedPredicate.Compile()(personTwo), Is.True, "Second person is matched");
      Assert.That(() => composedPredicate.Compile()(personThree), Is.False, "Third person is not matched");
    }

    [Test,AutoMoqData]
    public void Or_creates_predicate_which_satisfies_both_predicates(Person personOne,
                                                                     Person personTwo,
                                                                     Person personThree)
    {
      // Arrange
      personOne.Name = "Billy";
      personTwo.Name = "Bob";
      personThree.Name = "Thornton";

      var firstPredicate = Predicate.Create<Person>(p => p.Name.EndsWith("y", StringComparison.InvariantCulture));
      var secondPredicate = Predicate.Create<Person>(p => p.Name.EndsWith("n", StringComparison.InvariantCulture));

      // Act
      var composedPredicate = firstPredicate.Or(secondPredicate);

      // Assert
      Assert.That(() => composedPredicate.Compile()(personOne), Is.True, "First person is matched");
      Assert.That(() => composedPredicate.Compile()(personTwo), Is.False, "Second person is not matched");
      Assert.That(() => composedPredicate.Compile()(personThree), Is.True, "Third person is matched");
    }

    [Test,AutoMoqData]
    public void Not_negates_a_predicate(Person personOne,
                                        Person personTwo,
                                        Person personThree)
    {
      // Arrange
      personOne.Name = "Billy";
      personTwo.Name = "Bob";
      personThree.Name = "Thornton";

      var predicate = Predicate.Create<Person>(p => p.Name.EndsWith("y", StringComparison.InvariantCulture));

      // Act
      var composedPredicate = predicate.Not();

      // Assert
      Assert.That(() => composedPredicate.Compile()(personOne), Is.False, "First person is not matched");
      Assert.That(() => composedPredicate.Compile()(personTwo), Is.True, "Second person is matched");
      Assert.That(() => composedPredicate.Compile()(personThree), Is.True, "Third person is matched");
    }
  }
}
