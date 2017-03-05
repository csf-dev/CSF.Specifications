//
// InMemoryItem.cs
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

namespace CSF.Data
{
  /// <summary>
  /// Represents an item in an <see cref="InMemoryQuery"/>
  /// </summary>
  public class InMemoryItem
  {
    /// <summary>
    /// Gets the item type.
    /// </summary>
    /// <value>The type.</value>
    public Type Type { get; private set; }

    /// <summary>
    /// Gets the item identity.
    /// </summary>
    /// <value>The identity.</value>
    public object Identity { get; private set; }

    /// <summary>
    /// Gets the item.
    /// </summary>
    /// <value>The item.</value>
    public object Item { get; private set; }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Data.InMemoryItem"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      return Type.GetHashCode() ^ Identity.GetHashCode();
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Data.InMemoryItem"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Data.InMemoryItem"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Data.InMemoryItem"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      var other = obj as InMemoryItem;

      if(other == null)
      {
        return false;
      }

      return Type.Equals(other.Type) && Identity.Equals(other.Identity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.InMemoryItem"/> class.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="identity">Identity.</param>
    /// <param name="item">Item.</param>
    internal InMemoryItem(Type type, object identity, object item)
    {
      if(type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }
      if(identity == null)
      {
        throw new ArgumentNullException(nameof(identity));
      }
      if(item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }

      Type = type;
      Identity = identity;
      Item = item;
    }
  }
}

