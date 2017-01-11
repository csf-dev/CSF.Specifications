//
// InMemoryQuery.cs
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
using System.Linq;
using System.Collections.Generic;

namespace CSF.Data
{
  /// <summary>
  /// An implementation of <see cref="IQuery"/> which represents a transient in-memory data-set.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is intended for the purpose of mocking data-sets and queries (IE: it is a test fake).
  /// </para>
  /// </remarks>
  public class InMemoryQuery : IQuery
  {
    #region fields

    private ISet<InMemoryItem> _items;

    #endregion

    #region IQuery implementation

    /// <summary>
    /// Creates an instance of the given object-type, based upon a theory that it exists in the underlying data-source.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will always return a non-null object instance, even if the underlying object does not exist in the
    /// data source.  If a 'thoery object' is created for an object which does not actually exist, then an exception
    /// could be thrown if that theory object is used.
    /// </para>
    /// </remarks>
    /// <param name="identityValue">The identity value for the object to retrieve.</param>
    /// <typeparam name="TQueried">The type of object to retrieve.</typeparam>
    public virtual TQueried Theorise<TQueried>(object identityValue) where TQueried : class
    {
      return Get<TQueried>(identityValue);
    }

    /// <summary>
    /// Gets a single instance from the underlying data source, identified by an identity value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will either get an object instance, or it will return <c>null</c> (if no instance is found).
    /// </para>
    /// </remarks>
    /// <param name="identityValue">The identity value for the object to retrieve.</param>
    /// <typeparam name="TQueried">The type of object to retrieve.</typeparam>
    public virtual TQueried Get<TQueried>(object identityValue) where TQueried : class
    {
      if(identityValue == null)
      {
        throw new ArgumentNullException(nameof(identityValue));
      }

      return GetItemsOfType<TQueried>()
        .Where(x => identityValue.Equals(x.Identity))
        .Select(x => x.Item)
        .Cast<TQueried>()
        .FirstOrDefault();
    }

    /// <summary>
    /// Gets a new queryable data-source.
    /// </summary>
    /// <typeparam name="TQueried">The type of queried-for object.</typeparam>
    public virtual IQueryable<TQueried> Query<TQueried>() where TQueried : class
    {
      return GetItemsOfType<TQueried>()
        .Select(x => x.Item)
        .Cast<TQueried>()
        .ToArray()
        .AsQueryable();
    }

    #endregion

    #region public API

    /// <summary>
    /// Adds a single item to the in-memory query.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="identity">Identity.</param>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <returns>A reference to the current instance, such that method calls may be chained.</returns>
    public virtual InMemoryQuery Add<TItem>(TItem item, object identity) where TItem : class
    {
      if(item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }
      if(identity == null)
      {
        throw new ArgumentNullException(nameof(identity));
      }

      var inMemoryItem = new InMemoryItem(item.GetType(), identity, item);
      _items.Add(inMemoryItem);

      return this;
    }

    /// <summary>
    /// Adds a collection of items to the in-memory query.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="identitySelector">A function which selects the identity from each item.</param>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <returns>A reference to the current instance, such that method calls may be chained.</returns>
    public virtual InMemoryQuery Add<TItem>(IEnumerable<TItem> items,
                                            Func<TItem,object> identitySelector) where TItem : class
    {
      if(items == null)
      {
        throw new ArgumentNullException(nameof(items));
      }
      if(identitySelector == null)
      {
        throw new ArgumentNullException(nameof(identitySelector));
      }

      foreach(var item in items)
      {
        Add(item, identitySelector(item));
      }

      return this;
    }

    /// <summary>
    /// Gets the contents of the current instance for inspection.
    /// </summary>
    /// <returns>The contents.</returns>
    public virtual IEnumerable<InMemoryItem> GetContents()
    {
      return _items.AsEnumerable();
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets in-memory items of the requested type.
    /// </summary>
    /// <returns>The items which match the requested type.</returns>
    /// <typeparam name="TQueried">The requested item type.</typeparam>
    private IQueryable<InMemoryItem> GetItemsOfType<TQueried>() where TQueried : class
    {
      return _items.Where(x => typeof(TQueried).IsAssignableFrom(x.Type)).AsQueryable();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.InMemoryQuery"/> class.
    /// </summary>
    public InMemoryQuery()
    {
      _items = new HashSet<InMemoryItem>();
    }

    #endregion
  }

}

