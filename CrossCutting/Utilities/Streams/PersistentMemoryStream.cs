using System.IO;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// MemoryStream which doesn't close even if it's closed.
	/// Usable with cascade of streams. For example <see cref="System.IO.Compression.GZipStream"/> flushes
	/// all date into inner stream only on close, but it also closes inner stream. It's fine
	/// it inner stream is <see cref="FileStream"/> but it can be a problem
	/// if inner stream is <see cref="MemoryStream"/>.
	/// </summary>
	public class PersistentMemoryStream: MemoryStream
	{
		private bool m_AllowClose/* = false */;

		/// <summary>
		/// Gets or sets a value indicating whether stream should close when <see cref="Close()"/> is called.
		/// </summary>
		/// <value><c>true</c> if closing is allowed; otherwise, <c>false</c>.</value>
		public bool AllowClose
		{
			get { return m_AllowClose; }
			set { m_AllowClose = value; }
		}

		/// <summary>
		/// Closes the current stream and releases any resources (such as sockets and 
		/// file handles) associated with the current stream.
		/// if <see cref="AllowClose"/> is <c>false</c> it does nothing.
		/// </summary>
		public override void Close()
		{
			if (AllowClose)
				base.Close();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.IO.MemoryStream"/>
		/// and optionally releases the managed resources.
		/// Sets <see cref="AllowClose"/> to <c>true</c>.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged 
		/// resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			m_AllowClose = true;
			base.Dispose(disposing);
		}

		/// <summary>
		/// Rewinds this stream to position 0.
		/// </summary>
		public PersistentMemoryStream Rewind()
		{
			Position = 0;
			return this;
		}
	}
}
