using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// A stream which handles writes in separate thread. Used in producer/consumer 
	/// situation when writing to a stream is slow and benefits from doing actual
	/// writes in separate thread.
	/// </summary>
	public class AsyncOutputStream
        : Stream
	{
		#region consts

		/// <summary>Default buffer size for small writes (like 1 byte).</summary>
		private const int BUFFER_SIZE = 4096;

		#endregion

		#region fields

		/// <summary>Inner stream.</summary>
		private readonly Stream _other;

		/// <summary>Thread handling writes.</summary>
		private readonly Thread _thread;

		/// <summary>A queue of byte block to be written.</summary>
		private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();

		/// <summary>Semaphore to count ready-to-write blocks.</summary>
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

		/// <summary>A token indicating that the stream has been closed no and no further writes are possible.</summary>
		private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

		/// <summary>A flag indicating that all writes has been completed.</summary>
		private readonly ManualResetEventSlim _finished = new ManualResetEventSlim();

		/// <summary>Exception raised in writer loop.</summary>
		private Exception _exception; // = null;

		/// <summary>Flag indicating if exception was already reported.</summary>
		private bool _exceptionReported;

		/// <summary>Flag indicating if stream is isolated from underlying stream.</summary>
		private readonly bool _isolated;

		/// <summary>Flaf indicating of stream has been disposed.</summary>
		private bool _disposed; // = false;

		/// <summary>Buffer.</summary>
		private byte[] _buffer; // = null;

		/// <summary>Number of bytes in buffer.</summary>
		private int _length; // = 0;

		/// <summary>Number of bytes written to stream.</summary>
		private long _streamLength; // = 0;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="AsyncOutputStream"/> class.</summary>
		/// <param name="other">The other.</param>
		/// <param name="isolated">if set to <c>true</c> this stream will be isolated 
		/// from underlying stream, and won't dispose it when is disposed itself.</param>
		public AsyncOutputStream(Stream other, bool isolated = false)
		{
			if (other == null)
				throw new ArgumentNullException("other", "other is null.");
			_other = other;
			_isolated = isolated;
			_thread = new Thread(Loop);
			_thread.Start();
		}

		#endregion

		#region private implementation

		/// <summary>Enqeues a buffer.</summary>
		private void Enqeue()
		{
			if (_buffer != null)
			{
				Debug.Assert(_length == _buffer.Length);
				_queue.Enqueue(_buffer);
				_buffer = null;
				_length = 0;
				_semaphore.Release();
			}
		}

		/// <summary>Dequeues the specified buffer.</summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns><c>true</c> if operation succeeded, <c>false</c> if there is nothing 
		/// to write and stream has been closed.</returns>
		private bool Dequeue(out byte[] buffer)
		{
			bool canceled = false;
			while (true)
			{
				if (!canceled)
				{
					try
					{
						// wait for item or cancellation
						_semaphore.Wait(_cancel.Token);
					}
					catch (OperationCanceledException)
					{
						// it cancelled don't quit immediately, check again for remaing items
						canceled = true;
					}
				}

				if (_queue.TryDequeue(out buffer))
				{
					// if there are items in queue don't bother checking cancellation, 
					// they need to be processed
					return true;
				}
				else
				{
					// it queue is empty and it has been already cancelled - quit
					// if it it just empty - wait for new items
					if (canceled)
					{
						return false;
					}
				}
			}
		}

		/// <summary>Write loop.</summary>
		private void Loop()
		{
			try
			{
				byte[] buffer;
				while (Dequeue(out buffer))
				{
					_other.Write(buffer, 0, buffer.Length);
				}
			}
			catch (Exception e)
			{
				_exception = e;
			}
			finally
			{
				_finished.Set();
			}
		}

		/// <summary>Reports the loop exception.</summary>
		/// <param name="operation">The operation.</param>
		/// <param name="disposing">if set to <c>true</c> stream is disposing and does 
		/// not need to report exception if it was already reported.</param>
		private void ReportLoopException(string operation, bool disposing)
		{
			if (_exception != null)
			{
				if (_exceptionReported)
				{
					if (disposing)
					{
						// do nothing - allow dispose if exception has been reported already
					}
					else
					{
						throw new InvalidOperationException(
							string.Format("Operation '{0}' due to previous errors", operation),
							_exception);
					}
				}
				else
				{
					try
					{
						throw new AggregateException("Exception occured in async write loop.", _exception);
					}
					finally
					{
						_exceptionReported = true;
					}
				}
			}
		}

		/// <summary>Create NotSupportedException but does not throw it.</summary>
		/// <param name="operation">The operation name.</param>
		/// <returns>NotSupportedException</returns>
		private static NotSupportedException NotSupported(string operation)
		{
			return new NotSupportedException(
				string.Format("Operation '{0}' is not allowed", operation));
		}

		/// <summary>Gets a value indicating whether this instance is closed and no 
		/// further writes are possible.</summary>
		/// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
		private bool IsClosed { get { return _disposed || _cancel.IsCancellationRequested; } }

		#endregion

		#region overrides

		/// <summary>Gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>true if the stream supports reading; otherwise, false.</returns>
		public override bool CanRead { get { return false; } }

		/// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>true if the stream supports seeking; otherwise, false.</returns>
		public override bool CanSeek { get { return false; } }

		/// <summary>Gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		public override bool CanWrite { get { return true; } }

		/// <summary>Gets a value that determines whether the current stream can time out.</summary>
		/// <returns>A value that determines whether the current stream can time out.</returns>
		public override bool CanTimeout { get { return false; } }

		/// <summary>Gets the length in bytes of the stream.</summary>
		/// <returns>A long value representing the length of the stream in bytes.</returns>
		/// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
		public override long Length { get { return Interlocked.Read(ref _streamLength); } }

		/// <summary>Gets or sets the position within the current stream.</summary>
		/// <returns>The current position within the stream.</returns>
		/// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
		public override long Position
		{
			get { return Interlocked.Read(ref _streamLength); }
			set { throw NotSupported("SetPosition"); }
		}

		/// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		public override int Read(byte[] buffer, int offset, int count) { throw NotSupported("Read"); }

		/// <summary>Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
		/// <returns>The unsigned byte cast to an Int32, or -1 if at the end of the stream.</returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		public override int ReadByte() { throw NotSupported("ReadByte"); }

		/// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
		/// <returns>The new position within the current stream.</returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
		public override long Seek(long offset, SeekOrigin origin) { throw NotSupported("Seek"); }

		/// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
		public override void SetLength(long value) { throw NotSupported("SetLength"); }

		#endregion

		#region public interface

		/// <summary>
		/// Clears all buffers for this stream and causes any buffered data to be 
		/// written to the underlying device.
		/// </summary>
		public override void Flush()
		{
			if (_exception != null) throw new AggregateException(_exception);

			if (_length > 0)
			{
				var buffer = new byte[_length];
				Buffer.BlockCopy(_buffer, 0, buffer, 0, _length);
				_buffer = buffer;
				Enqeue();
			}
		}

		/// <summary>Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0) return;
			if (IsClosed) throw new ObjectDisposedException("AsyncOutputStream");
			if (buffer == null) throw new ArgumentNullException("buffer");
			if (offset < 0) throw new ArgumentOutOfRangeException("offset");
			if (count < 0) throw new ArgumentOutOfRangeException("count");
			if (offset + count > buffer.Length) throw new ArgumentException("Sum of offset and count are greater than buffer length.");

			while (count > 0)
			{
				if (_exception != null) ReportLoopException("Write", false);

				if (_length == 0)
				{
					if (count >= BUFFER_SIZE)
					{
						var left = (count / BUFFER_SIZE) * BUFFER_SIZE;
						_buffer = new byte[left];
						Buffer.BlockCopy(buffer, offset, _buffer, 0, left);
						_length = left;
						Enqeue();
						offset += left;
						count -= left;
					}
					else
					{
						_buffer = new byte[BUFFER_SIZE];
						Buffer.BlockCopy(buffer, offset, _buffer, 0, count);
						_length = count;
						offset += count;
						count = 0;
					}
				}
				else
				{
					var left = BUFFER_SIZE - _length;
					if (count >= left)
					{
						Buffer.BlockCopy(buffer, offset, _buffer, _length, left);
						_length += left;
						offset += left;
						count -= left;
						Enqeue();
					}
					else
					{
						Buffer.BlockCopy(buffer, offset, _buffer, _length, count);
						_length += count;
						offset += count;
						count = 0;
					}
				}
			}
			Interlocked.Add(ref _streamLength, count);
		}

		/// <summary>Writes a byte to the current position in the stream and advances the position within the stream by one byte.</summary>
		/// <param name="value">The byte to write to the stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public override void WriteByte(byte value)
		{
			if (IsClosed) throw new ObjectDisposedException("AsyncOutputStream");
			if (_exception != null) ReportLoopException("WriteByte", false);

			if (_length == 0)
			{
				_buffer = new byte[BUFFER_SIZE];
				_buffer[0] = value;
				_length = 1;
			}
			else
			{
				var left = BUFFER_SIZE - _length - 1;
				Debug.Assert(left >= 0);
				_buffer[_length++] = value;
				if (left == 0) Enqeue();
			}
			Interlocked.Increment(ref _streamLength);
		}

		/// <summary>
		/// Closes the current stream and releases any resources (such as sockets and 
		/// file handles) associated with the current stream.
		/// </summary>
		public override void Close()
		{
			Flush();
			_cancel.Cancel();
			_finished.Wait();
			base.Close();
			if (!_isolated) _other.Close();
			ReportLoopException("Close", true);
		}

		#endregion

		#region protected implementation

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> 
		/// and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; 
		/// false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!_disposed)
				{
					_disposed = true;
					if (_cancel != null) _cancel.Dispose();
					if (_semaphore != null) _semaphore.Dispose();
					if (_finished != null) _finished.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
