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
using NUnit.Framework;
using System.Linq;
using CSF.Specifications.Tests.Stubs;

namespace CSF.Specifications.Tests.Specifications
{
    [TestFixture, Parallelizable]
    public class QueryableExtensionsTests
    {
        [Test, AutoMoqData]
        public void Where_filters_by_spec()
        {
            Assert.That(() => GetCollection().Where(GetSut("Anna")).FirstOrDefault()?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void First_uses_spec()
        {
            Assert.That(() => GetCollection().First(GetSut("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void FirstOrDefault_uses_spec()
        {
            Assert.That(() => GetCollection().FirstOrDefault(GetSut("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Single_uses_spec()
        {
            Assert.That(() => GetCollection().Single(GetSut("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void SingleOrDefault_uses_spec()
        {
            Assert.That(() => GetCollection().SingleOrDefault(GetSut("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Count_uses_spec()
        {
            Assert.That(() => GetCollection().Count(GetSut("Anna")), Is.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void Any_uses_spec()
        {
            Assert.That(() => GetCollection().Any(GetSut("Anna")), Is.True);
        }

        ISpecificationExpression<Person> GetSut(string name) => new PersonNameSpecificationExpression(name);

        IQueryable<Person> GetCollection()
        {
            var personOne = new Person
            {
                Identity = 1,
                Name = "Bob",
            };
            var personTwo = new Person
            {
                Identity = 2,
                Name = "Anna",
            };
            var personThree = new Person
            {
                Identity = 3,
                Name = "Jo",
            };
            return new[] { personOne, personTwo, personThree }.AsQueryable();
        }
    }
}
