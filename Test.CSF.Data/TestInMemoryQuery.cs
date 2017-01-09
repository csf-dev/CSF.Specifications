//
// TestInMemoryQuery.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2017 Craig Fowler
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
using NUnit.Framework;
using CSF.Data;
using Test.CSF.Data.Stubs;
using Ploeh.AutoFixture;
using System.Linq;

namespace Test.CSF.Data
{
  [TestFixture]
  public class TestInMemoryQuery
  {
    #region fields

    private IFixture _autofixture;
    private InMemoryQuery _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      _sut = new InMemoryQuery();
    }

    #endregion

    #region tests

    [Test]
    public void Add_single_adds_item()
    {
      // Arrange
      var item = new Person() { Identity = _autofixture.Create<long>() };

      // Act
      _sut.Add(item, item.Identity);

      // Assert
      var added = _sut.GetContents();
      Assert.AreEqual(1, added.Count(), "Item count");
      var addedItem = added.Single();

      Assert.AreEqual(typeof(Person), addedItem.Type, "Item type");
      Assert.AreEqual(item.Identity, addedItem.Identity, "Item identity");
      Assert.AreSame(item, addedItem.Item, "Item is same instance");
    }

    [Test]
    public void Add_same_item_twice_does_not_create_duplicates()
    {
      // Arrange
      var item = new Person() { Identity = _autofixture.Create<long>() };

      // Act
      _sut
        .Add(item, item.Identity)
        .Add(item, item.Identity);

      // Assert
      var added = _sut.GetContents();
      Assert.AreEqual(1, added.Count());
    }

    [Test]
    public void Add_multiple_adds_multiple_items()
    {
      // Arrange
      var items = new [] {
        new Person() { Identity = _autofixture.Create<long>() },
        new Person() { Identity = _autofixture.Create<long>() },
      };

      // Act
      _sut.Add(items, x => x.Identity);

      // Assert
      var added = _sut.GetContents();
      Assert.AreEqual(2, added.Count(), "Item count");

      var addedItems = added.Select(x => x.Item).ToArray();
      CollectionAssert.AreEquivalent(items, addedItems, "All items added");
    }

    [Test]
    public void Query_exposes_added_items()
    {
      // Arrange
      var items = new [] {
        new Person() { Identity = _autofixture.Create<long>() },
        new Person() { Identity = _autofixture.Create<long>() },
      };
      _sut.Add(items, x => x.Identity);

      // Act
      var result = _sut.Query<Person>();

      // Assert
      CollectionAssert.AreEquivalent(items, result.ToArray());
    }

    [Test]
    public void Query_does_not_expose_items_of_other_types()
    {
      // Arrange
      var people = new [] {
        new Person() { Identity = _autofixture.Create<long>() },
        new Person() { Identity = _autofixture.Create<long>() },
      };
      var uris = new [] {
        new Uri("/one/two"),
        new Uri("/three/four"),
      };
      _sut
        .Add(people, x => x.Identity)
        .Add(uris, x => x.AbsoluteUri);

      // Act
      var result = _sut.Query<Person>();

      // Assert
      CollectionAssert.AreEquivalent(people, result.ToArray());
    }

    [Test]
    public void Get_retrieves_correct_item()
    {
      // Arrange
      var items = new [] {
        new Person() { Identity = _autofixture.Create<long>() },
        new Person() { Identity = _autofixture.Create<long>() },
      };
      _sut.Add(items, x => x.Identity);

      // Act
      var result = _sut.Get<Person>(items[1].Identity);

      // Assert
      Assert.AreSame(items[1], result);
    }

    #endregion
  }
}

