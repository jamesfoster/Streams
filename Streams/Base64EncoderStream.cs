namespace Streams
{
	using System;
	using System.IO;
	using System.Text;

	public class Base64EncoderStream : Stream
	{
		const int InputBufferSize = 3;
		const int OutputBufferSize = 4;

		readonly byte[] inputBuffer = new byte[InputBufferSize];
		readonly byte[] outputBuffer = new byte[OutputBufferSize];
		int outputPos;

		public Stream InnerStream { get; private set; }

		public Base64EncoderStream(Stream inner)
		{
			InnerStream = inner;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var pos = 0;

			if (outputPos > 0)
			{
				if (count <= OutputBufferSize - outputPos)
				{
					Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, count);
					outputPos += count;
					if(outputPos == OutputBufferSize)
						outputPos = 0;
					return count;
				}

				pos += OutputBufferSize - outputPos;
				Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, pos);
			}

			outputPos = 0;

			while(pos < count)
			{
				var read = InnerStream.Read(inputBuffer, 0, InputBufferSize);

				if(read == 0)
					return pos;

				if(count - pos > OutputBufferSize)
				{
					ConvertInputToBase64(read, buffer, offset + pos);
					pos += OutputBufferSize;
				}
				else
				{
					ConvertInputToBase64(read, outputBuffer, 0);
					outputPos = count - pos;
					Buffer.BlockCopy(outputBuffer, 0, buffer, offset + pos, count - pos);
					pos += outputPos;
					break;
				}
			}

			return pos;
		}

		void ConvertInputToBase64(int length, byte[] outputArray, int outputOffset)
		{
			var temp = new char[OutputBufferSize];
			var outputLength = Convert.ToBase64CharArray(inputBuffer, 0, length, temp, 0);

			Encoding.ASCII.GetBytes(temp, 0, outputLength, outputArray, outputOffset);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if(disposing)
				InnerStream.Dispose();
		}

		public override void Flush()
		{
			InnerStream.Flush();
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
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
			set { throw new NotSupportedException("Base64Stream does not support setting Position."); }
		}

		#region "Not Supported"

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Base64Stream does not support seeking.");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("Base64Stream does not support SetLength.");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Base64Stream does not support writing.");
		}

		#endregion
	}
}
