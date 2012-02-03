namespace Streams
{
	using System;
	using System.IO;
	using System.Text;

	public enum Base64DecodeMode { 
		IgnoreWhiteSpaces = 0,
		DoNotIgnoreWhiteSpaces = 1, 
	}

	public class Base64DecoderStream : ReadOnlyStreamWrapper
	{
		const int InputBufferSize = 4;
		const int OutputBufferSize = 3;

		readonly byte[] inputBuffer = new byte[InputBufferSize];
		readonly byte[] outputBuffer = new byte[OutputBufferSize];
		int outputPos;
		int outputSize;

		public Base64DecodeMode Whitespaces { get; private set; }

		public Base64DecoderStream(Stream inner, Base64DecodeMode whitespaces = Base64DecodeMode.IgnoreWhiteSpaces)
			: base(inner)
		{
			Whitespaces = whitespaces;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var pos = 0;

			if (outputPos > 0)
			{
				if (count <= outputSize - outputPos)
				{
					Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, count);
					outputPos += count;
					if(outputPos == outputSize)
						outputPos = 0;
					return count;
				}

				pos += outputSize - outputPos;
				Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, pos);
			}

			outputPos = 0;

			while(pos < count)
			{
				var inputPos = 0;
				while(inputPos < InputBufferSize)
				{
					var read = InnerStream.Read(inputBuffer, inputPos, 1);

					if(read == 0)
					{
						if(inputPos == 0)
							return pos;

						throw new InvalidOperationException("Unexpected end of base64 string.");
					}

					if(!SkipWhitespace((char)inputBuffer[inputPos]))
						inputPos++;
				}

				outputSize = ConvertInputFromBase64(InputBufferSize, outputBuffer, 0);

				if(count - pos > outputSize)
				{
					Buffer.BlockCopy(outputBuffer, 0, buffer, offset + pos, outputSize);
					pos += outputSize;
				}
				else
				{
					outputPos = count - pos;
					Buffer.BlockCopy(outputBuffer, 0, buffer, offset + pos, count - pos);
					pos += outputPos;
					break;
				}
			}

			return pos;
		}

		int ConvertInputFromBase64(int length, byte[] outputArray, int outputOffset)
		{
			var temp = new char[InputBufferSize];
			Encoding.ASCII.GetChars(inputBuffer, 0, InputBufferSize, temp, 0);

			var bytes = Convert.FromBase64CharArray(temp, 0, length);

			Buffer.BlockCopy(bytes, 0, outputArray, outputOffset, bytes.Length);

			return bytes.Length;
		}

		bool SkipWhitespace(char c)
		{
			return Whitespaces == Base64DecodeMode.IgnoreWhiteSpaces && char.IsWhiteSpace(c);
		}
	}
}