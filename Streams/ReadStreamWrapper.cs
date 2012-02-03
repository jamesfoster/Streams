namespace Streams
{
	using System;
	using System.IO;

	public abstract class ReadOnlyStreamWrapper : ReadOnlyStream
	{
		public Stream InnerStream { get; protected set; }

		protected ReadOnlyStreamWrapper(Stream innerStream)
		{
			if (innerStream == null)
				throw new ArgumentNullException("innerStream");

			if(!innerStream.CanRead)
				throw new ArgumentException("Input stream is not readable", "innerStream");

			InnerStream = innerStream;
		}

		public override bool CanRead
		{
			get { return InnerStream.CanRead; }
		}

		public override long Length
		{
			get
			{
				return InnerStream.Length;
			}
		}

		public override long Position
		{
			get { return InnerStream.Position; }
			set { throw new NotSupportedException(string.Format("{0} does not support setting Position.", GetType().Name)); }
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && InnerStream != null)
					InnerStream.Dispose();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		public override void Flush()
		{
			InnerStream.Flush();
		}
	}
}