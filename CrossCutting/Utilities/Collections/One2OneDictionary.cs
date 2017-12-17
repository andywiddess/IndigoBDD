using System;
using System.Collections.Generic;
using System.Collections;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Represents a two way lookup dictionary - AKA a reversable dictionary. The side is unimportant
	/// </summary>
	/// <typeparam name="A">The type of side A</typeparam>
	/// <typeparam name="B">The type of side B</typeparam>
	public class One2OneDictionary<A, B>
	{
		#region Member Variables

		private readonly Dictionary<A, B> m_a2b = new Dictionary<A, B>();
		private readonly Dictionary<B, A> m_b2a = new Dictionary<B, A>();

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds the specified A and B relationship
		/// </summary>
		/// <param name="a">The A object</param>
		/// <param name="b">The B object</param>
		public void Add(A a, B b)
		{
			m_a2b.Add(a, b);
			m_b2a.Add(b, a);
		}

		/// <summary>
		/// Determines whether the dictionary contains an A object
		/// </summary>
		/// <param name="a">The A object</param>
		/// <returns>
		/// 	<c>true</c> if the dictionary contains the specified A; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsA(A a)
		{
			return m_a2b.ContainsKey(a);
		}

		/// <summary>
		/// Determines whether the dictionary contains any type of object
		/// </summary>
		/// <param name="any">Any.</param>
		/// <returns>
		/// 	<c>true</c> if dictionary contains the specified object; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(object any)
		{
			if (any is A)
			{
				if (m_a2b.ContainsKey((A)any))
					return true;
			}

			if (any is B)
			{
				if (m_a2b.ContainsKey((A)any))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified B object
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns>
		/// 	<c>true</c> if the dictionary contains the specified B; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsB(B b)
		{
			return m_b2a.ContainsKey(b);
		}

		/// <summary>
		/// Finds the pair for a specified object.
		/// </summary>
		/// <param name="oneSide">The one side.</param>
		/// <exception cref="ArgumentOutOfRangeException">If the object <c>oneSide</c> doesn't exist</exception>
		/// <returns>The pair for the specified object</returns>
		public object Pair(object oneSide)
		{
			if (oneSide is A)
			{
				B outVal;
				if (m_a2b.TryGetValue((A)oneSide, out outVal))
					return outVal;
			}

			if (oneSide is B)
			{
				A outVal;
				if (m_b2a.TryGetValue((B)oneSide, out outVal))
					return outVal;
			}

			throw new ArgumentOutOfRangeException("No pair for item in collection.");
		}

		/// <summary>
		/// Deletes the specified A object
		/// </summary>
		/// <param name="a">The A object</param>
		public void Delete(A a)
		{
			if (ContainsA(a))
			{
				B b = m_a2b[a];
				m_a2b.Remove(a);
				m_b2a.Remove(b);
			}
		}

		/// <summary>
		/// Deletes the specified B object.
		/// </summary>
		/// <param name="b">The B object</param>
		public void Delete(B b)
		{
			if (ContainsB(b))
			{
				A a = m_b2a[b];
				m_b2a.Remove(b);
				m_a2b.Remove(a);
			}
		}

		/// <summary>
		/// Finds the A object for a specified B object
		/// </summary>
		/// <param name="b">The specified B object</param>
		/// <exception cref="ArgumentOutOfRangeException">If the <c>b</c> object doesn't exist.</exception>
		/// <returns>The matching A object</returns>
		public A FindA(B b)
		{
			if (ContainsB(b))
				return m_b2a[b];
			else
				throw new ArgumentOutOfRangeException("No pair for item in collection.");
		}

		/// <summary>
		/// Finds the B object for a specified A object
		/// </summary>
		/// <param name="a">The specified A object</param>
		/// <exception cref="ArgumentOutOfRangeException">If the <c>a</c> object doesn't exist.</exception>
		/// <returns>The matching B object</returns>
		public B FindB(A a)
		{
			if (ContainsA(a))
				return m_a2b[a];
			else
				throw new ArgumentOutOfRangeException("No pair for item in collection.");
		}

		/// <summary>
		/// Clears the dictionary
		/// </summary>
		public void Clear()
		{
			m_b2a.Clear();
			m_a2b.Clear();
		}

		/// <summary>
		/// Gets the <typeparamref name="B"/> with the specified a.
		/// </summary>
		/// <value></value>
		public B this[A a]
		{
			get
			{
				return FindB(a);
			}
		}

		/// <summary>
		/// Gets the <typeparamref name="A"/> with the specified b.
		/// </summary>
		/// <value></value>
		public A this[B b]
		{
			get
			{
				return FindA(b);
			}
		}

		/// <summary>
		/// Keyses this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<T> Keys<T>()
		{
			if (typeof(A).IsAssignableFrom(typeof(T)))
			{
				foreach (T a in (IEnumerable)m_a2b.Keys)
				{
					yield return a;
				}
			}
			if (typeof(B).IsAssignableFrom(typeof(T)))
			{
				foreach (T b in (IEnumerable)m_b2a.Keys)
				{
					yield return b;
				}
			}
		}

		#endregion
	}
}
