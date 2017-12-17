using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Represents an event arguement which takes a generic type(s) of data
	/// </summary>
	/// <typeparam name="TData">The type of generic data</typeparam>
	public class EventArgs<TData> : System.EventArgs
	{
		private TData m_data;

		/// <summary>
		/// Gets the EventArg's data.
		/// </summary>
		/// <value>The data.</value>
		public TData Data
		{
			get { return m_data; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData&gt;"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public EventArgs(TData data)
		{
			m_data = data;
		}
	}

	/// <summary>
	/// Event Arguments implementation which takes two generic data types
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	public class EventArgs<TData1, TData2> : System.EventArgs
	{
		private TData1 m_data1;
		private TData2 m_data2;

		/// <summary>
		/// Gets the first piece of data, called data1
		/// </summary>
		/// <value>The first piece of data.</value>
		public TData1 Data1
		{
			get { return m_data1; }
		}

		/// <summary>
		/// Gets the second piece of data, called data2
		/// </summary>
		/// <value>The second piece of data.</value>
		public TData2 Data2
		{
			get { return m_data2; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		public EventArgs(TData1 data1, TData2 data2)
		{
			m_data1 = data1;
			m_data2 = data2;
		}
	}

	/// <summary>
	/// Event Arguments implementation which takes two generic data types
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	/// <typeparam name="TData3">The type of the data3.</typeparam>
	public class EventArgs<TData1, TData2, TData3> : System.EventArgs
	{
		private TData1 m_data1;
		private TData2 m_data2;
		private TData3 m_data3;

		/// <summary>
		/// Gets the first piece of data, called data1
		/// </summary>
		/// <value>The first piece of data.</value>
		public TData1 Data1
		{
			get { return m_data1; }
		}

		/// <summary>
		/// Gets the second piece of data, called data2
		/// </summary>
		/// <value>The second piece of data.</value>
		public TData2 Data2
		{
			get { return m_data2; }
		}

		/// <summary>
		/// Gets the third piece of data, called data3
		/// </summary>
		/// <value>The third piece of data.</value>
		public TData3 Data3
		{
			get { return m_data3; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2, TData3&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		/// <param name="data3">The third piece of data</param>
		public EventArgs(TData1 data1, TData2 data2, TData3 data3)
		{
			m_data1 = data1;
			m_data2 = data2;
			m_data3 = data3;
		}
	}


	/// <summary>
	/// Event Arguments implementation which takes two generic data types
	/// </summary>
	/// <typeparam name="TData1">The type of the data1</typeparam>
	/// <typeparam name="TData2">The type of the data2</typeparam>
	/// <typeparam name="TData3">The type of the data3.</typeparam>
	/// <typeparam name="TData4">The type of the data4.</typeparam>
	public class EventArgs<TData1, TData2, TData3, TData4> : System.EventArgs
	{
		private TData1 m_data1;
		private TData2 m_data2;
		private TData3 m_data3;
		private TData4 m_data4;

		/// <summary>
		/// Gets the first piece of data, called data1
		/// </summary>
		/// <value>The first piece of data.</value>
		public TData1 Data1
		{
			get { return m_data1; }
		}

		/// <summary>
		/// Gets the second piece of data, called data2
		/// </summary>
		/// <value>The second piece of data.</value>
		public TData2 Data2
		{
			get { return m_data2; }
		}

		/// <summary>
		/// Gets the third piece of data, called data3
		/// </summary>
		/// <value>The third piece of data.</value>
		public TData3 Data3
		{
			get { return m_data3; }
		}

		/// <summary>
		/// Gets the fourth piece of data, called data4
		/// </summary>
		/// <value>The fourth piece of data.</value>
		public TData4 Data4
		{
			get { return m_data4; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;TData1, TData2, TData3&gt;"/> class.
		/// </summary>
		/// <param name="data1">The first piece of data.</param>
		/// <param name="data2">The second piece of data</param>
		/// <param name="data3">The third piece of data</param>
		/// <param name="data4">The fourth piece of data.</param>
		public EventArgs(TData1 data1, TData2 data2, TData3 data3, TData4 data4)
		{
			m_data1 = data1;
			m_data2 = data2;
			m_data3 = data3;
			m_data4 = data4;
		}
	}

}
