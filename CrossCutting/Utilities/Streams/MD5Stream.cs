using System;
using System.IO;
using System.Security.Cryptography;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Transparent stream which calculates MD5 on the fly.
	/// </summary>
	public class MD5Stream
        : GenericTransparentStream
	{
		#region fields

		/// <summary>
		/// File access mode.
		/// </summary>
		private FileAccess m_Access;

		/// <summary>
		/// MD5 calculator.
		/// </summary>
		private MD5 m_MD5;

		/// <summary>
		/// Indicates if MD5 checksum is ready.
		/// </summary>
		private bool m_MD5HashReady/* = false */;

		/// <summary>
		/// Indicates if stream is still valid. After MD5 is calculated it cannot be modified.
		/// It is not possible to have partial sums.
		/// </summary>
		private bool m_AllowUse = true;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MD5Stream"/> class.
		/// </summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="access">The access mode.</param>
		public MD5Stream(Stream innerStream, FileAccess access)
			: base(innerStream)
		{
			if (innerStream == null)
				throw new ArgumentNullException("innerStream", "innerStream is null.");
			CheckFileAccess(innerStream, access);

			m_Access = access;
			m_MD5 = MD5.Create();
		}

		#endregion

		#region overrides

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports seeking; otherwise, false.</returns>
		public override bool CanSeek
		{
			get { return false; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports reading; otherwise, false.</returns>
		public override bool CanRead
		{
			get { return (m_Access == FileAccess.Read) && (InnerStream.CanRead); }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		public override bool CanWrite
		{
			get { return (m_Access == FileAccess.Write) && (InnerStream.CanWrite); }
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
			get { return base.Position; }
			set { throw new NotSupportedException("Seek operation is not supported"); }
		}

		/// <summary>
		/// Sets the position within the inner stream. This method always throws an exception.
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
			throw new NotSupportedException("Seek operation is not supported");
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
			if (m_Access == FileAccess.Read)
			{
				throw new NotSupportedException("Setting length is not supported for stream opened in read mode");
			}
			base.SetLength(value);
		}

		#endregion

		#region utility

		/// <summary>
		/// Checks the file access mode.
		/// </summary>
		/// <param name="innerStream">The inner stream.</param>
		/// <param name="access">The access.</param>
		private void CheckFileAccess(Stream innerStream, FileAccess access)
		{
			if (access == FileAccess.Read)
			{
				if (!innerStream.CanRead)
					throw new ArgumentException("Stream opened for reading with inner stream not supporting it");
			}
			else if (access == FileAccess.Write)
			{
				if (!innerStream.CanWrite)
					throw new ArgumentException("Stream opened for writing with inner stream not supporting it");
			}
			else
			{
				throw new ArgumentException("Only Read or Write modes are allowed for MD5Stream");
			}
		}

		#endregion

		#region md5 calculation

		/// <summary>
		/// TransformFileBlock requires buffer, but we don't have more data
		/// so this is an buffer to transform - empty buffer
		/// </summary>
		private static readonly byte[] m_EmptyBuffer = new byte[0];

		/// <summary>
		/// to avoid allocation avery time ReadByte/WriteByte is called
		/// </summary>
		private byte[] m_OneByteBuffer = new byte[1];

		/// <summary>
		/// Calculates the MD5.
		/// </summary>
		/// <param name="oneByte">The one byte.</param>
		private void CalculateMD5(byte oneByte)
		{
			if (m_MD5HashReady) return;

			lock (m_OneByteBuffer)
			{

				m_OneByteBuffer[0] = oneByte;
				m_MD5.TransformBlock(m_OneByteBuffer, 0, 1, m_OneByteBuffer, 0);
			}
		}

		/// <summary>
		/// Calculates the MD5.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		private void CalculateMD5(byte[] buffer, int offset, int count)
		{
			if (m_MD5HashReady) return;

			m_MD5.TransformBlock(buffer, offset, count, buffer, offset);
		}

		/// <summary>
		/// Returns MD5 hash. Because of the way MD5 hash works, it will not be possible
		/// to hash more after this call. You can either allow further use of a stream
		/// but hash will not change.
		/// </summary>
		/// <param name="allowFurtherUse">if set to <c>true</c> further use of this stream will be allowed (but hash won't change anymore).</param>
		/// <returns>MD5 hash.</returns>
		public byte[] Hash(bool allowFurtherUse)
		{
			if (!m_MD5HashReady)
			{
				m_MD5.TransformBlock(m_EmptyBuffer, 0, 0, m_EmptyBuffer, 0);
				m_MD5.TransformFinalBlock(m_EmptyBuffer, 0, 0);
				m_MD5HashReady = true;
			}
			m_AllowUse = allowFurtherUse;

			return m_MD5.Hash;
		}

		#endregion

		#region checks

		/// <summary>
		/// Checks the use allowed.
		/// </summary>
		private void CheckUseAllowed()
		{
			if (!m_AllowUse)
			{
				throw new ArgumentException(string.Format(
						"No more {0} are allowed",
						m_Access == FileAccess.Read ? "reads" : "writes"));
			}
		}

		/// <summary>
		/// Checks the access allowed.
		/// </summary>
		/// <param name="neededAccess">The needed access.</param>
		private void CheckAccessAllowed(FileAccess neededAccess)
		{
			if (m_Access != neededAccess)
			{
				throw new NotSupportedException(string.Format(
						"Operation not supported for {0} only streams",
						m_Access == FileAccess.Read ? "read" : "write"));
			}
		}

		#endregion

		#region read

		/// <summary>
		/// Reads a sequence of bytes from the inner stream and advances the position 
		/// within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer 
		/// contains the specified byte array with the values between offset 
		/// and (offset + count - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in buffer at which 
		/// to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read 
		/// from the current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than 
		/// the number of bytes requested if that many bytes are not currently 
		/// available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			CheckAccessAllowed(FileAccess.Read);
			CheckUseAllowed();

			int result = base.Read(buffer, offset, count);

			CalculateMD5(buffer, offset, count);

			return result;
		}

		/// <summary>
		/// Reads a byte from the stream and advances the position within 
		/// the stream by one byte, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>
		/// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override int ReadByte()
		{
			CheckAccessAllowed(FileAccess.Read);
			CheckUseAllowed();

			int result = base.ReadByte();

			if (result != -1)
			{
				CalculateMD5((byte)result);
			}

			return result;
		}

		#endregion

		#region write

		/// <summary>
		/// Writes a sequence of bytes to the inner stream 
		/// and advances the current position within this stream 
		/// by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies 
		/// count bytes from buffer to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in buffer at 
		/// which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
		/// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			CheckAccessAllowed(FileAccess.Write);
			CheckUseAllowed();

			base.Write(buffer, offset, count);
			CalculateMD5(buffer, offset, count);
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
			CheckAccessAllowed(FileAccess.Write);
			CheckUseAllowed();

			base.WriteByte(value);
			CalculateMD5((byte)value);
		}

		#endregion
	}
}
