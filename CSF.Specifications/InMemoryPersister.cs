//
// InMemoryPersister.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
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
namespace CSF.Data
{
  /// <summary>
  /// In-memory implementation of <see cref="IPersister"/> which works upon an <see cref="InMemoryQuery"/>.
  /// </summary>
  public class InMemoryPersister : IPersister
  {
    readonly InMemoryQuery query;

    /// <summary>
    /// Adds the specified item to the data-store.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="identity">The item's identity.</param>
    /// <typeparam name="T">The item type.</typeparam>
    public virtual void Add<T>(T item, object identity) where T : class
    {
      query.Add(item, identity);
    }

    /// <summary>
    /// Deletes the specified item from the data-store.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="identity">The item's identity.</param>
    /// <typeparam name="T">The item type.</typeparam>
    public virtual void Delete<T>(T item, object identity) where T : class
    {
      query.Delete(item);
    }

    /// <summary>
    /// Updates the specified item in the data-store.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="identity">The item's identity.</param>
    /// <typeparam name="T">The item type.</typeparam>
    public virtual void Update<T>(T item, object identity) where T : class
    {
      // Intentional no-op
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Data.InMemoryPersister"/> class.
    /// </summary>
    /// <param name="query">Query.</param>
    public InMemoryPersister(InMemoryQuery query)
    {
      if(query == null)
        throw new ArgumentNullException(nameof(query));

      this.query = query;
    }
  }
}
