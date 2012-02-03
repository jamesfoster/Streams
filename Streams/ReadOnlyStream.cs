namespace Streams
{
	using System;
	using System.IO;

	public abstract class ReadOnlyStream : Stream
	{
		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(string.Format("{0} does not support seeking.", GetType().Name));
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException(string.Format("{0} does not support SetLength.", GetType().Name));
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(string.Format("{0} does not support writing.", GetType().Name));
		}
	}
}