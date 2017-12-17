namespace Indigo.CrossCutting.Utilities.Events
{

	// NOTE There is already a none-generic CancelEventArgs in the System.ComponentModel namespace.

	// NOTE:SWH The generic CancelEventArgs should be derived from System.ComponentModel.CancelEventArgs not from EventArgs<T>

	/// <summary>
	/// Represents an event argument which takes a generic type(s) of data and supports cancel
	/// </summary>
	/// <typeparam name="TData">The type of generic data</typeparam>
	public class CancelEventArgs<TData>: EventArgs<TData>
	{
		private bool m_cancel; // = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CancelEventArgs&lt;TData&gt;"/> is cancel.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData&gt;"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public CancelEventArgs(TData data)
			: base(data)
		{
		}
	}

	/// <summary>
	/// Event Arguments implementation which takes two generic data types and supports cancel
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	public class CancelEventArgs<TData1, TData2>: EventArgs<TData1, TData2>
	{
		private bool m_cancel = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CancelEventArgs&lt;TData1, TData2&gt;"/> is cancel.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		public CancelEventArgs(TData1 data1, TData2 data2)
			: base(data1, data2)
		{
		}
	}

	/// <summary>
	/// Event Arguments implementation which takes three generic data types and supports cancel
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	/// <typeparam name="TData3">The type of the data3.</typeparam>
	public class CancelEventArgs<TData1, TData2, TData3>: EventArgs<TData1, TData2, TData3>
	{
		private bool m_cancel = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CancelEventArgs&lt;TData1, TData2, TData3&gt;"/> is cancel.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2, TData3&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		/// <param name="data3">The third piece of data</param>
		public CancelEventArgs(TData1 data1, TData2 data2, TData3 data3)
			: base(data1, data2, data3)
		{
		}
	}


	/// <summary>
	/// Event Arguments implementation which takes four generic data types and supports cancel
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	/// <typeparam name="TData3">The type of the data3.</typeparam>
	/// <typeparam name="TData4">The type of the data4.</typeparam>
	public class CancelEventArgs<TData1, TData2, TData3, TData4>: EventArgs<TData1, TData2, TData3, TData4>
	{
		private bool m_cancel = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CancelEventArgs&lt;TData1, TData2, TData3, TData4&gt;"/> is cancel.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
		public bool Cancel
		{
			get { return m_cancel; }
			set { m_cancel = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2, TData3&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		/// <param name="data3">The third piece of data</param>
		/// <param name="data4">The fourth piece of data.</param>
		public CancelEventArgs(TData1 data1, TData2 data2, TData3 data3, TData4 data4)
			: base(data1, data2, data3, data4)
		{
		}
	}

}
