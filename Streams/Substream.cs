namespace Streams
{
	using System;
	using System.IO;

	public class Substream : ReadOnlyStreamWrapper
	{
		public long StartPosition { get; set; }
		public long SubLength { get; set; }

		long position;

		public Substream(Stream innerStream, long startPosition)
			: this(innerStream, startPosition, -1)
		{
		}

		public Substream(Stream innerStream, long startPosition, long length) : base(innerStream)
		{
			if (startPosition < 0) throw new ArgumentException("startPosition should be non-negative.", "startPosition");
			if (length < -1) throw new ArgumentException("length should be non-negative or -1.", "length");

			StartPosition = startPosition;
			SubLength = length;
			position = -1;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var pos = 0;

			if (position == -1)
			{
				MoveToStartPosition();
				position = 0;
			}

			if (SubLength != -1 && position + count > SubLength)
				count = (int) (SubLength - position);

			while (pos < count)
			{
				var read = InnerStream.Read(buffer, offset + pos, count - pos);

				if (read == 0)
					return pos;

				pos += read;
				position += read;
			}

			return pos;
		}

		void MoveToStartPosition()
		{
			if (InnerStream.CanSeek)
				InnerStream.Seek(StartPosition, SeekOrigin.Begin);
			else
			{
				var count = StartPosition;
				var b = 0;

				while (--count > 0 && b != -1)
				{
					b = InnerStream.ReadByte();
				}
			}
		}

		public override long Position
		{
			get { return position < 0 ? 0 : position; }
			set { base.Position = value; }
		}

		public override long Length
		{
			get
			{
				return SubLength;
			}
		}
	}
}