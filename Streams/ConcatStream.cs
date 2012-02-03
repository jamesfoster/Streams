namespace Streams
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class ConcatStream : ReadOnlyStream
	{
		public Stream[] InnerStreams;

		int currentStreamIndex;
		int position;
		long? length;
		bool canRead;

		public ConcatStream(IEnumerable<Stream> streams)
		{
			if (streams == null) throw new ArgumentNullException("streams");

			Init(streams.ToArray());
		}

		public ConcatStream(params Stream[] streams)
		{
			if (streams == null) throw new ArgumentNullException("streams");

			Init(streams.ToArray());
		}

		void Init(Stream[] streams)
		{
			InnerStreams = streams;
			position = 0;
			currentStreamIndex = 0;
			canRead = true;
			try
			{
				length = InnerStreams.Sum(s => s.Length);
			}
			catch
			{
				length = null;
			}
		}

		public override void Flush()
		{
			// What to do?
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if(currentStreamIndex == InnerStreams.Length)
				return 0;

			var pos = 0;

			var currentStream = InnerStreams[currentStreamIndex];

			while(pos < count)
			{
				var read = currentStream.Read(buffer, offset + pos, count - pos);
				position += read;
				pos += read;

				if(read == 0)
				{
					if(++currentStreamIndex == InnerStreams.Length)
						return pos;

					currentStream = InnerStreams[currentStreamIndex];
				}
			}

			return pos;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					foreach (var innerStream in InnerStreams)
					{
						innerStream.Dispose();
					}
				}
			}
			finally
			{
				InnerStreams = null;
				canRead = false;

				base.Dispose(disposing);
			}
		}

		public override bool CanRead
		{
			get { return canRead; }
		}

		public override long Length
		{
			get
			{
				if (length.HasValue)
					return length.Value;

				throw new NotSupportedException(string.Format("{0} does not support seeking.", GetType().Name));
			}
		}

		public override long Position
		{
			get { return position; }
			set { throw new NotSupportedException(string.Format("{0} does not support setting Position.", GetType().Name)); }
		}
	}
}