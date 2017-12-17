using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// IEnumerable which collects items asynchronously in separate thread.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class AsyncEnumerable<T>: IEnumerable<T>
	{
		#region class Preloaded<T>

		/// <summary>Enumerable which starts enumerating instantly, without waiting for GetEnumerator().</summary>
		public class PreloadedAsyncEnumerable: IEnumerable<T>
		{
			#region fields

			/// <summary>Internal lock.</summary>
			private readonly object m_Lock = new object();

			/// <summary>Original enumerable.</summary>
			private readonly AsyncEnumerable<T> m_Enumerable;

			/// <summary>Preloaded enumerator.</summary>
			private IEnumerator<T> m_Enumerator;

			#endregion

			#region constructor

			/// <summary>Initializes a new instance of the <see cref="AsyncEnumerable&lt;T&gt;.PreloadedAsyncEnumerable"/> class.</summary>
			/// <param name="enumerable">The enumerable.</param>
			internal PreloadedAsyncEnumerable(AsyncEnumerable<T> enumerable)
			{
				if (enumerable == null)
					throw new ArgumentNullException("enumerable", "enumerable is null.");

				m_Enumerable = enumerable;
				m_Enumerator = m_Enumerable.GetEnumerator();
			}

			#endregion

			#region IEnumerable<T> Members

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
			public IEnumerator<T> GetEnumerator()
			{
				lock (m_Lock)
				{
					if (m_Enumerator != null)
					{
						var result = m_Enumerator;
						m_Enumerator = null;
						return result;
					}
					else
					{
						return m_Enumerable.GetEnumerator();
					}
				}
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion
		}

		#endregion

		#region fields

		/// <summary>'Real' enumerable.</summary>
		private readonly IEnumerable<T> m_Internal;

		/// <summary>
		/// Maximum size of internal queue.
		/// </summary>
		private readonly int m_MaximumSize;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncEnumerable&lt;T&gt;"/> class over other IEnumerable. 
		/// Enumeration thread does not start immediately, it starts when <see cref="GetEnumerator"/> is called.
		/// </summary>
		/// <param name="other">The other enumerable.</param>
		public AsyncEnumerable(IEnumerable<T> other)
			: this(other, int.MaxValue)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncEnumerable&lt;T&gt;"/> class over other IEnumerable.
		/// Enumeration thread does not start immediately, it starts when <see cref="GetEnumerator"/> is called.
		/// </summary>
		/// <param name="other">The other enumerable.</param>
		/// <param name="maximumSize">The maximum queue size.</param>
		public AsyncEnumerable(IEnumerable<T> other, int maximumSize)
		{
			if (other == null)
				throw new ArgumentNullException("other", "Cannot be null.");
			m_Internal = other;
			m_MaximumSize = maximumSize;
		}

		#endregion

		#region public interface

		/// <summary>Starts enumerating instantly in background.</summary>
		/// <returns>Enumerable.</returns>
		public PreloadedAsyncEnumerable Preload()
		{
			return new PreloadedAsyncEnumerable(this);
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new AsyncEnumerator<T>(m_Internal.GetEnumerator(), m_MaximumSize);
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
