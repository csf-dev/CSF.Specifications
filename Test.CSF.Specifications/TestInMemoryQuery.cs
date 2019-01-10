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
  [TestFixture,Parallelizable]
  public class TestInMemoryQuery
  {
    #region tests

    [Test,AutoMoqData]
    public void Add_single_adds_item(Person item,
                                     long identity,
                                     InMemoryQuery sut)
    {
      // Arrange
      item.Identity = identity;

      // Act
      sut.Add(item, item.Identity);

      // Assert
      var added = sut.GetContents();
      Assert.AreSame(item, added.Single().Item);
    }

    [Test,AutoMoqData]
    public void Add_uses_item_real_type(Employee item,
                                        long identity,
                                        InMemoryQuery sut)
    {
      // Arrange
      item.Identity = identity;
      Person castItem = item;

      // Act
      sut.Add(castItem, castItem.Identity);

      // Assert
      var added = sut.GetContents();
      Assert.AreEqual(typeof(Employee), added.Single().Type);
    }

    [Test,AutoMoqData]
    public void Add_same_item_twice_does_not_create_duplicates(Person item,
                                                               long identity,
                                                               InMemoryQuery sut)
    {
      // Arrange
      item.Identity = identity;

      // Act
      sut
        .Add(item, item.Identity)
        .Add(item, item.Identity);

      // Assert
      var added = sut.GetContents();
      Assert.AreEqual(1, added.Count());
    }

    [Test,AutoMoqData]
    public void Add_multiple_adds_multiple_items(Person item1,
                                                 long identity1,
                                                 Person item2,
                                                 long identity2,
                                                 InMemoryQuery sut)
    {
      // Arrange
      item1.Identity = identity1;
      item2.Identity = identity2;
      var items = new [] { item1, item2 };

      // Act
      sut.Add(items, x => x.Identity);

      // Assert
      var added = sut.GetContents().Select(x => x.Item).ToArray();
      CollectionAssert.AreEquivalent(items, added);
    }

    [Test,AutoMoqData]
    public void Query_exposes_added_items(Person item1,
                                          long identity1,
                                          Person item2,
                                          long identity2,
                                          InMemoryQuery sut)
    {
      // Arrange
      item1.Identity = identity1;
      item2.Identity = identity2;
      var items = new [] { item1, item2 };

      sut.Add(items, x => x.Identity);

      // Act
      var result = sut.Query<Person>();

      // Assert
      CollectionAssert.AreEquivalent(items, result.ToArray());
    }

    [Test,AutoMoqData]
    public void Query_does_not_expose_items_of_other_types(Person item1,
                                                           long identity1,
                                                           Person item2,
                                                           long identity2,
                                                           Animal animal1,
                                                           long animalIdentity1,
                                                           Animal animal2,
                                                           long animalIdentity2,
                                                           InMemoryQuery sut)
    {
      // Arrange
      item1.Identity = identity1;
      item2.Identity = identity2;
      var items = new [] { item1, item2 };

      animal1.Identity = animalIdentity1;
      animal2.Identity = animalIdentity2;
      var animals = new [] { animal1, animal2 };

      sut
        .Add(items, x => x.Identity)
        .Add(animals, x => x.Identity);

      // Act
      var result = sut.Query<Person>();

      // Assert
      CollectionAssert.AreEquivalent(items, result.ToArray());
    }

    [Test,AutoMoqData]
    public void Get_retrieves_correct_item(Person item1,
                                           long identity1,
                                           Person item2,
                                           long identity2,
                                           InMemoryQuery sut)
    {
      // Arrange
      item1.Identity = identity1;
      item2.Identity = identity2;
      var items = new [] { item1, item2 };

      sut.Add(items, x => x.Identity);

      // Act
      var result = sut.Get<Person>(identity1);

      // Assert
      Assert.AreSame(item1, result);
    }

    [Test,AutoMoqData]
    public void Delete_removes_the_item(Person item,
                                        long identity,
                                        InMemoryQuery sut)
    {
      // Arrange
      item.Identity = identity;
      sut.Add(item, item.Identity);

      // Act
      sut.Delete<Person>(identity);

      // Assert
      var contents = sut.GetContents();
      Assert.IsEmpty(contents);
    }


    #endregion
  }
}

