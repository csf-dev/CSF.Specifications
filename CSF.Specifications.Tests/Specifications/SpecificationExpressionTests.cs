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
using NUnit.Framework;
using CSF.Specifications.Tests.Stubs;
using System.Linq;

namespace CSF.Specifications.Tests.Specifications
{
    [TestFixture, Parallelizable]
    public class SpecificationExpressionTests
    {
        [Test, AutoMoqData]
        public void AsPredicate_returns_predicate_which_matches_a_matching_object(Person person)
        {
            // Arrange
            person.Name = "Bob";
            var spec = GetSut("Bob");

            // Act
            var result = spec.AsPredicate()(person);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test, AutoMoqData]
        public void AsPredicate_returns_predicate_which_does_not_match_a_non_matching_object(Person person)
        {
            // Arrange
            person.Name = "Anna";
            var spec = GetSut("Bob");

            // Act
            var result = spec.AsPredicate()(person);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test, AutoMoqData]
        public void Not_returns_specification_which_matches_a_non_matching_object(Person person)
        {
            // Arrange
            person.Name = "Anna";
            var spec = GetSut("Bob");

            // Act
            var result = spec.Not().Matches(person);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test, AutoMoqData]
        public void Not_returns_specification_which_does_not_match_a_matching_object(Person person)
        {
            // Arrange
            person.Name = "Bob";
            var spec = GetSut("Bob");

            // Act
            var result = spec.Not().Matches(person);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test, AutoMoqData]
        public void Matches_returns_true_when_instance_is_match(Person person)
        {
            // Arrange
            person.Name = "Bob";
            var spec = GetSut("Bob");

            // Act
            var result = spec.Matches(person);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_when_instance_is_not_match(Person person)
        {
            // Arrange
            person.Name = "Anna";
            var spec = GetSut("Bob");

            // Act
            var result = spec.Matches(person);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test, AutoMoqData]
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

            var people = new[] { personOne, personTwo, personThree }.AsQueryable();

            var result = people.Where(combinedSpec).ToArray();

            Assert.That(result, Is.EquivalentTo(new[] { personTwo }));
        }

        [Test, AutoMoqData]
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

            var people = new[] { personOne, personTwo, personThree }.AsQueryable();

            // Act
            var result = people.Where(combinedSpec).ToArray();

            // Assert
            Assert.That(result, Is.EquivalentTo(new[] { personTwo, personThree }));
        }

        [Test, AutoMoqData]
        public void And_a_function_creates_combined_function_which_must_satisfy_both(Person personOne,
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

            var firstSpec = new PersonIdentifierSpecificationExpression(2);
            var secondSpec = new PersonNameSpecificationFunction("Anna");

            var combinedSpec = firstSpec.And(secondSpec);

            var people = new[] { personOne, personTwo, personThree };

            var result = people.Where(combinedSpec).ToArray();

            Assert.That(result, Is.EquivalentTo(new[] { personTwo }));
        }

        [Test, AutoMoqData]
        public void Or_a_function_creates_combined_function_which_must_satisfy_either(Person personOne,
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

            var firstSpec = new PersonIdentifierSpecificationExpression(3);
            var secondSpec = new PersonNameSpecificationFunction("Anna");

            var combinedSpec = firstSpec.Or(secondSpec);

            var people = new[] { personOne, personTwo, personThree };

            // Act
            var result = people.Where(combinedSpec).ToArray();

            // Assert
            Assert.That(result, Is.EquivalentTo(new[] { personTwo, personThree }));
        }

        [Test, AutoMoqData]
        public void Transform_may_create_a_transformed_matching_specification_expression_for_a_new_type(Pet pet, string name)
        {
            var spec = Spec.Expr<Person>(x => x.Name == name);
            var transformed = spec.Transform(t => t.To<Pet>(x => x.Owner));
            pet.Owner.Name = name;
            Assert.That(() => transformed.Matches(pet), Is.True);
        }

        [Test, AutoMoqData]
        public void Transform_may_create_a_transformed_non_matching_specification_expression_for_a_new_type(Pet pet)
        {
            var spec = Spec.Expr<Person>(x => x.Name == "One");
            var transformed = spec.Transform(t => t.To<Pet>(x => x.Owner));
            pet.Owner.Name = "Two";
            Assert.That(() => transformed.Matches(pet), Is.False);
        }

        ISpecificationExpression<Person> GetSut(string name) => new PersonNameSpecificationExpression(name);
    }
}
