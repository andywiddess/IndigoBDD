using System;
using System.IO;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Generic stream which passes all calls to internal stream.
	/// Used as a base to Streams slightly modifying behavior
	/// if other streams. For example, see <see cref="MD5Stream"/>.
	/// </summary>
	public class GenericTransparentStream
        : Stream
	{
		#region fields

		private Stream m_InnerStream;

		#endregion

		#region properties

		/// <summary>
		/// Gets the inner stream.
		/// </summary>
		/// <value>The inner stream.</value>
		public Stream InnerStream
		{
			get { return m_InnerStream; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTransparentStream"/> class.
		/// Initializes inner stream.
		/// </summary>
		/// <param name="innerStream">The inner stream.</param>
		public GenericTransparentStream(Stream innerStream)
		{
			if (innerStream == null)
				throw new ArgumentNullException("innerStream", "innerStream is null.");
			m_InnerStream = innerStream;
		}

		#endregion

		#region overriden properties

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports reading; otherwise, false.</returns>
		public override bool CanRead
		{
			get { return m_InnerStream.CanRead; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports seeking; otherwise, false.</returns>
		public override bool CanSeek
		{
			get { return m_InnerStream.CanSeek; }
		}

		/// <summary>
		/// Gets a value that determines whether the current stream can time out.
		/// </summary>
		/// <value></value>
		/// <returns>A value that determines whether the current stream can time out.</returns>
		public override bool CanTimeout
		{
			get { return m_InnerStream.CanTimeout; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		public override bool CanWrite
		{
			get { return m_InnerStream.CanWrite; }
		}

		/// <summary>
		/// When overridden in a derived class, gets the length in bytes of the stream.
		/// </summary>
		/// <value></value>
		/// <returns>A long value representing the length of the stream in bytes.</returns>
		/// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override long Length
		{
			get { return m_InnerStream.Length; }
		}

		/// <summary>
		/// When overridden in a derived class, gets or sets the position within the current stream.
		/// </summary>
		/// <value></value>
		/// <returns>The current position within the stream.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override long Position
		{
			get { return m_InnerStream.Position; }
			set { m_InnerStream.Position = value; }
		}

		/// <summary>
		/// Gets or sets a value that determines how long the stream will attempt to read before timing out.
		/// </summary>
		/// <value></value>
		/// <returns>A value that determines how long the stream will attempt to read before timing out.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout"></see> method always throws an <see cref="T:System.InvalidOperationException"></see>. </exception>
		public override int ReadTimeout
		{
			get { return m_InnerStream.ReadTimeout; }
			set { m_InnerStream.ReadTimeout = value; }
		}

		/// <summary>
		/// Gets or sets a value that determines how long the stream will attempt to write before timing out.
		/// </summary>
		/// <value></value>
		/// <returns>A value that determines how long the stream will attempt to write before timing out.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout"></see> method always throws an <see cref="T:System.InvalidOperationException"></see>. </exception>
		public override int WriteTimeout
		{
			get { return m_InnerStream.WriteTimeout; }
			set { m_InnerStream.WriteTimeout = value; }
		}

		#endregion

		#region overriden methods

		/// <summary>
		/// Closes the inner stream.
		/// </summary>
		public override void Close()
		{
			m_InnerStream.Close();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"></see> and optionally releases the managed resources.
		/// Disposes inner stream.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; 
		/// <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_InnerStream.Dispose();
				m_InnerStream = null;
			}
		}

		/// <summary>
		/// Flushes inner stream.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		public override void Flush()
		{
			m_InnerStream.Flush();
		}

		/// <summary>
		/// Reads a sequence of bytes from the inner stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return m_InnerStream.Read(buffer, offset, count);
		}

		/// <summary>
		/// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>
		/// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override int ReadByte()
		{
			return m_InnerStream.ReadByte();
		}

		/// <summary>
		/// Sets the position within the inner stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the origin parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating 
		/// the reference point used to obtain the new position.</param>
		/// <returns>
		/// The new position within the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking, 
		/// such as if the stream is constructed from a pipe or console output. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after 
		/// the stream was closed. </exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return m_InnerStream.Seek(offset, origin);
		}

		/// <summary>
		/// Sets the length of the inner stream.
		/// </summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.NotSupportedException">The stream does not support 
		/// both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after 
		/// the stream was closed. </exception>
		public override void SetLength(long value)
		{
			m_InnerStream.SetLength(value);
		}

		/// <summary>
		/// Writes a sequence of bytes to the inner stream and advances the current position 
		/// within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
		/// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			m_InnerStream.Write(buffer, offset, count);
		}

		/// <summary>
		/// Writes a byte to the current position in the inner stream and 
		/// advances the position within the stream by one byte.
		/// </summary>
		/// <param name="value">The byte to write to the stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing, 
		/// or the stream is already closed. </exception>
		public override void WriteByte(byte value)
		{
			m_InnerStream.WriteByte(value);
		}

		#endregion
	}
}
