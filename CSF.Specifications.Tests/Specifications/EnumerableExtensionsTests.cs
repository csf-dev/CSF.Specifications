﻿//
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
using System.Collections.Generic;

namespace CSF.Specifications.Tests.Specifications
{
    [TestFixture, Parallelizable]
    public class EnumerableExtensionsTests
    {
        [Test, AutoMoqData]
        public void Where_filters_by_spec_expression()
        {
            Assert.That(() => GetCollection().Where(GetSpecExpression("Anna")).FirstOrDefault()?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void First_uses_spec_expression()
        {
            Assert.That(() => GetCollection().First(GetSpecExpression("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void FirstOrDefault_uses_spec_expression()
        {
            Assert.That(() => GetCollection().FirstOrDefault(GetSpecExpression("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Single_uses_spec_expression()
        {
            Assert.That(() => GetCollection().Single(GetSpecExpression("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void SingleOrDefault_uses_spec_expression()
        {
            Assert.That(() => GetCollection().SingleOrDefault(GetSpecExpression("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Count_uses_spec_expression()
        {
            Assert.That(() => GetCollection().Count(GetSpecExpression("Anna")), Is.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void Any_uses_spec_expression()
        {
            Assert.That(() => GetCollection().Any(GetSpecExpression("Anna")), Is.True);
        }

        [Test, AutoMoqData]
        public void Where_filters_by_spec_function()
        {
            Assert.That(() => GetCollection().Where(GetSpecFunction("Anna")).FirstOrDefault()?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void First_uses_spec_function()
        {
            Assert.That(() => GetCollection().First(GetSpecFunction("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void FirstOrDefault_uses_spec_function()
        {
            Assert.That(() => GetCollection().FirstOrDefault(GetSpecFunction("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Single_uses_spec_function()
        {
            Assert.That(() => GetCollection().Single(GetSpecFunction("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void SingleOrDefault_uses_spec_function()
        {
            Assert.That(() => GetCollection().SingleOrDefault(GetSpecFunction("Anna"))?.Identity, Is.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void Count_uses_spec_function()
        {
            Assert.That(() => GetCollection().Count(GetSpecFunction("Anna")), Is.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void Any_uses_spec_function()
        {
            Assert.That(() => GetCollection().Any(GetSpecFunction("Anna")), Is.True);
        }

        ISpecificationExpression<Person> GetSpecExpression(string name) => new PersonNameSpecificationExpression(name);

        ISpecificationFunction<Person> GetSpecFunction(string name) => new PersonNameSpecificationFunction(name);

        IEnumerable<Person> GetCollection()
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
            return new[] { personOne, personTwo, personThree };
        }
    }
}
