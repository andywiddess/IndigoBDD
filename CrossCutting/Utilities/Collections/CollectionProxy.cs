using System;
using System.Collections.Generic;
using System.Linq;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Collection proxy. All <see cref="ICollection&lt;T&gt;"/> methods are implemented as callbacks.
	/// If you are going to reuse a collection many times, it's much better to define a separate type.
	/// But sometimes it's just easier to expose data structures as collections without writing a new class,
	/// but just writing callback methods.
	/// For example: internally values are stored as CSV but have to be exposed as ICollection&lt;string&gt;
	/// without defining new class.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class CollectionProxy<T>: ICollection<T>
	{
		#region fields

		/// <summary>
		/// Is collection read-only. Doesn't have to be set, but setting it upfront make 
		/// some operations faster.
		/// </summary>
		private bool? m_ReadOnly;

		#endregion

		#region delegates

		/// <summary>
		/// CopyTo handler.
		/// </summary>
		public delegate void CopyToAction(T[] array, int arrayIndex);

		/// <summary>
		/// Add handler.
		/// </summary>
		public Action<T> HandleAdd;

		/// <summary>
		/// Clear handler.
		/// </summary>
		public Action HandleClear;

		/// <summary>
		/// Contains handler. Not required.
		/// </summary>
		public Converter<T, bool> HandleContains;

		/// <summary>
		/// Remove handler
		/// </summary>
		public Converter<T, bool> HandleRemove;

		/// <summary>
		/// CopyTo handler. Not required.
		/// </summary>
		public CopyToAction HandleCopyTo;

		/// <summary>
		/// Count handler. Not required.
		/// </summary>
		public Func<int> HandleCount;

		/// <summary>
		/// IsReadOnly handler. Not required.
		/// </summary>
		public Func<bool> HandleReadOnly;

		/// <summary>
		/// Enumerate handler. Not required but recommended.
		/// </summary>
		public Func<IEnumerable<T>> HandleEnumerate;

		/// <summary>
		/// AsCollection handler. Optional. If you can provide inner collection object
		/// lot of things will be much easier.
		/// </summary>
		public Func<ICollection<T>> HandleCollection;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionProxy&lt;T&gt;"/> class.
		/// </summary>
		public CollectionProxy()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="readOnly">Indicates if collection is read only or not.</param>
		public CollectionProxy(bool readOnly)
		{
			m_ReadOnly = readOnly;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public CollectionProxy(ICollection<T> collection)
		{
			HandleCollection = delegate { return collection; };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="readOnly">Indicates if collection is read only or not.</param>
		public CollectionProxy(ICollection<T> collection, bool readOnly)
		{
			HandleCollection = delegate { return collection; };
			m_ReadOnly = readOnly;
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Uses <see cref="HandleAdd"/> or <see cref="HandleCollection"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException("Cannot perform Add on ReadOnly collection");
			}
			else if (HandleAdd != null)
			{
				HandleAdd(item);
			}
			else if (HandleCollection != null)
			{
				HandleCollection().Add(item);
			}
			else
			{
				throw new InvalidOperationException("Cannot Add to collection. No handlers defined.");
			}
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Uses <see cref="HandleClear"/> or <see cref="HandleCollection"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException("Cannot perform Clear on ReadOnly collection");
			}
			else if (HandleClear != null)
			{
				HandleClear();
			}
			else if (HandleCollection != null)
			{
				HandleCollection().Clear();
			}
			else
			{
				throw new InvalidOperationException("Cannot Clear collection. No handlers defined.");
			}
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// Uses <see cref="HandleContains"/> or <see cref="HandleCollection"/> or <see cref="HandleEnumerate"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			if (HandleContains != null)
			{
				return HandleContains(item);
			}
			else if (HandleCollection != null)
			{
				return HandleCollection().Contains(item);
			}
			else if (HandleEnumerate != null)
			{
				return HandleEnumerate().Any((i) => Patterns.Equals(i, item));
			}
			else
			{
				throw new InvalidOperationException("Cannot check if collection Contains item. Handlers not defined.");
			}
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// Uses <see cref="HandleCopyTo"/> or <see cref="HandleCollection"/> or <see cref="HandleEnumerate"/>.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (HandleCopyTo != null)
			{
				HandleCopyTo(array, arrayIndex);
			}
			else if (HandleCollection != null)
			{
				HandleCollection().CopyTo(array, arrayIndex);
			}
			else if (HandleEnumerate != null)
			{
				foreach (T item in HandleEnumerate()) array[arrayIndex++] = item;
			}
			else
			{
				throw new InvalidOperationException("Cannot Copy collection. Handlers not defined.");
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Uses <see cref="HandleCount"/> or <see cref="HandleCollection"/> or <see cref="HandleEnumerate"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get
			{
				if (HandleCount != null)
				{
					return HandleCount();
				}
				else if (HandleCollection != null)
				{
					return HandleCollection().Count;
				}
				else if (HandleEnumerate != null)
				{
					return HandleEnumerate().Count();
				}
				else
				{
					throw new InvalidOperationException("Cannot Count items. Handlers not defined.");
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// Uses internally set value or <see cref="HandleReadOnly"/> or <see cref="HandleCollection"/>.
		/// </summary>
		/// <value>A value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				if (m_ReadOnly.HasValue)
				{
					return m_ReadOnly.Value;
				}
				else if (HandleReadOnly != null)
				{
					return HandleReadOnly();
				}
				else if (HandleCollection != null)
				{
					return HandleCollection().IsReadOnly;
				}
				else
				{
					return false;
				}
			}
			set
			{
				m_ReadOnly = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Uses <see cref="HandleRemove"/> or <see cref="HandleCollection"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item)
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException("Cannot perform Remove on ReadOnly collection");
			}
			else if (HandleRemove != null)
			{
				return HandleRemove(item);
			}
			else if (HandleCollection != null)
			{
				return HandleCollection().Remove(item);
			}
			else
			{
				throw new InvalidOperationException("Cannot Remove item. Handlers not defined.");
			}
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (HandleEnumerate != null)
			{
				return HandleEnumerate().GetEnumerator();
			}
			else if (HandleCollection != null)
			{
				return HandleCollection().GetEnumerator();
			}
			else
			{
				throw new InvalidOperationException("Cannot Enumerate collection. Handlers not defined");
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
