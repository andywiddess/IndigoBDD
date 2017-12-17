using System;
using System.Diagnostics;
using Indigo.CrossCutting.Utilities.Events;
using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Base class for FixedRef collection family. Do not use it. It work only with assumption that
	/// <code><![CDATA[class FixedRefBase<TCollection> : TCollection { ... }]]></code> which cannot be coded
	/// (C# syntactic limitation).
	/// Use <see cref="FixedRefEnumerable{T}"/>, <see cref="FixedRefCollection{T}"/>, <see cref="FixedRefList{T}"/>,
	/// <see cref="FixedRefDictionary{K,V}"/> or <see cref="FixedRefSet{T}"/> instead.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <typeparam name="TSelf">The type of derived class.</typeparam>
	public abstract class FixedRefBase<TCollection, TSelf> // can't be coded but it should inherit from TCollection
		where TCollection: class
		where TSelf: FixedRefBase<TCollection, TSelf>, TCollection
	{
		#region fields

		private readonly Func<TCollection> m_Getter;
		private readonly Action<TCollection> m_Setter;
		private TCollection m_Data;
		private bool m_RefreshNeeded = true;

		#endregion

		#region events

		/// <summary>Occurs when binding is changing.</summary>
		public event EventHandler<ChangingEventArgs<TCollection>> BindingChanging;

		/// <summary>Occurs when binding changed.</summary>
		public event EventHandler<ChangedEventArgs<TCollection>> BindingChanged;

		#endregion

		#region properties

		/// <summary>Gets or sets the data. Do not use this value for exposing a list, 
		/// use <see cref="Expose()"></see> instead.</summary>
		/// <value>The data.</value>
		public TCollection Data
		{
			get 
			{
				if ((m_RefreshNeeded || Volatile) && (m_Getter != null))
				{
					var value = m_Getter();
					if (!object.ReferenceEquals(m_Data, value)) SetData(value);
					m_RefreshNeeded = false;
				}
				return m_Data;
			}
			set 
			{
				if (!object.ReferenceEquals(m_Data, value))
				{
					SetData(value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether original collection reference is volatile.
		/// In such case, reference is read every time collection is accessed. If not, it gets cached.
		/// It still can be reread by invoking <see cref="Invalidate"/>. The default for <c>Volatile</c> is
		/// <c>false</c>.</summary>
		/// <value><c>true</c> if volatile; otherwise, <c>false</c>.</value>
		public bool Volatile { get; set; }

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="FixedRefBase{TCollection,TSelf}"/> class.</summary>
		public FixedRefBase()
		{
			TypeCheck();
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefBase{TCollection,TSelf}"/> class.</summary>
		/// <param name="collection">The collection.</param>
		public FixedRefBase(TCollection collection)
		{
			TypeCheck();
			m_Data = collection;
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefBase{TCollection,TSelf}"/> class.</summary>
		public FixedRefBase(Func<TCollection> getter, Action<TCollection> setter)
		{
			TypeCheck();
			m_Getter = getter;
			m_Setter = setter;
			m_Data = (m_Getter == null) ? null : m_Getter();
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefBase{TCollection,TSelf}"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefBase(TCollection collection, Func<TCollection> getter, Action<TCollection> setter)
		{
			TypeCheck();
			m_Getter = getter;
			m_Setter = setter;
			m_Data = (m_Getter == null) ? collection : m_Getter();
		}

		#endregion

		#region private implementation

		/// <summary>Sets the base collection reference.</summary>
		/// <param name="value">The value.</param>
		private void SetData(TCollection value)
		{
			bool allow = true;

			if (BindingChanging != null)
			{
				var args = new ChangingEventArgs<TCollection>(m_Data, value);
				BindingChanging(this, args);
				allow = !args.Cancel;
			}

			if (allow)
			{
				var old_value = m_Data;
				m_Data = value;

				if (m_Setter != null)
				{
					m_Setter(m_Data);
				}

				if (BindingChanged != null)
				{
					BindingChanged(this, new ChangedEventArgs<TCollection>(old_value, value));
				}
			}
		}

		/// <summary>Checks collection type. Does what should be done on compile-time but it can't be.</summary>
		[Conditional("DEBUG")]
		private void TypeCheck()
		{
			// this assert is done only in debug
			// it detects if derived class inherits from TCollection
			// FixedRefBase does not but requires that derived class does.
			if (!typeof(TCollection).IsAssignableFrom(GetType()))
			{
				throw new ArgumentException(
					"Type '{0}' does not inherit from '{1}'".With(GetType().Name, typeof(TCollection).Name));
			}
		}

		#endregion

		#region public interface

		/// <summary>Exposes the collection. Use this method if you are exposing collection to "outside world".</summary>
		/// <returns>Collection.</returns>
		public TCollection Expose()
		{
			// this is a little bit dangerous (the cast) 
			// because we can't enforce that FixedRefBase<TCollection> inherits from TCollection
			return Data == null ? null : (TCollection)((object)this);
		}

		/// <summary>Exposes the collection. Use this method if you are exposing collection to "outside world".</summary>
		/// <param name="subject">The subject.</param>
		/// <returns>
		/// Collection.
		/// </returns>
		public TCollection Expose(TCollection subject)
		{
			Data = subject;
			return Expose();
		}

		/// <summary>Invalidates the reference to collection. Next time collection is invoked reference will be 
		/// reread.</summary>
		public void Invalidate()
		{
			m_RefreshNeeded = true;
		}

		#endregion
	}
}