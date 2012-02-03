namespace Streams
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public class RegexFindReplaceStream : ReadOnlyStreamWrapper
	{
		public IDictionary<Regex, Func<Match, string>> Replacements { get; private set; }
		public int MaxMatchLength { get; set; }
		public Encoding Encoding { get; private set; }

		int BufferSize { get; set; }
		int Increment { get; set; }

		byte[] inputBuffer;
		readonly byte[] outputBuffer;
		int inputSize;
		int outputSize;
		int outputPos;

		Replacement NextReplacement { get; set; }

		RegexFindReplaceStream(Stream inner, int maxMatchLength, Encoding encoding)
			: base(inner)
		{
			BufferSize = maxMatchLength * 2;
			Increment = MaxMatchLength = maxMatchLength;
			Encoding = encoding;

			outputBuffer = new byte[Increment];
		}

		public RegexFindReplaceStream(Stream inner, IDictionary<Regex, string> replacements, int maxMatchLength, Encoding encoding)
			: this(inner, maxMatchLength, encoding)
		{
			Replacements = new Dictionary<Regex, Func<Match, string>>();

			foreach (var replacement in replacements)
			{
				var r = replacement;
				Replacements.Add(r.Key, m => r.Value);
			}
		}

		public RegexFindReplaceStream(Stream inner, IDictionary<string, string> replacements, int maxMatchLength, Encoding encoding)
			: this(inner, maxMatchLength, encoding)
		{
			Replacements = new Dictionary<Regex, Func<Match, string>>();

			foreach (var replacement in replacements)
			{
				var r = replacement;
				Replacements.Add(new Regex(r.Key), m => r.Value);
			}
		}

		public RegexFindReplaceStream(Stream inner, IDictionary<Regex, Func<Match, string>> replacements, int maxMatchLength, Encoding encoding)
			: this(inner, maxMatchLength, encoding)
		{
			Replacements = replacements;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var pos = 0;

			if (inputBuffer == null)
			{
				InitializeInputBuffer();
			}

			while (pos < count)
			{
				if(HasFinished())
					return pos;

				var read = DoRead(buffer, offset + pos, count - pos);
				pos += read;

				if(pos < count)
					CheckAndProgress();
			}

			return pos;
		}

		int DoRead(byte[] buffer, int offset, int count)
		{
			var pos = 0;

			if (outputSize > 0)
			{
				if (count + outputPos < outputSize)
				{
					Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, count);
					outputPos += count;
					return count;
				}

				Buffer.BlockCopy(outputBuffer, outputPos, buffer, offset, outputSize - outputPos);
				pos = outputSize - outputPos;

				outputPos = outputSize = 0;
			}

			if (NextReplacement != null && pos < count)
			{
				count -= pos;
				var rPos = NextReplacement.Position;
				var rLength = NextReplacement.Bytes.Length;

				if(count + rPos < rLength)
				{
					Buffer.BlockCopy(NextReplacement.Bytes, rPos, buffer, offset + pos, count);
					NextReplacement.Position += count;
					return count + pos;
				}

				Buffer.BlockCopy(NextReplacement.Bytes, rPos, buffer, offset + pos, rLength - rPos);
				pos += rLength - rPos;

				NextReplacement = null;
			}

			return pos;
		}

		bool HasFinished()
		{
			return inputSize == 0 && outputSize == 0 && NextReplacement == null;
		}

		void CheckAndProgress()
		{
			var position = Increment;
			var length = 0;
			string replaceWith = null;
			var s = Encoding.GetString(inputBuffer, 0, inputSize);

			foreach (var replacement in Replacements)
			{
				var match = replacement.Key.Match(s);
				if (match.Success && match.Index < position)
				{
					length = match.Length;
					replaceWith = replacement.Value(match);
					position = match.Index;
				}
				if(position == 0)
					break;
			}

			Buffer.BlockCopy(inputBuffer, 0, outputBuffer, 0, Increment);
			outputSize = position;
			outputPos = 0;

			if(outputSize > inputSize)
				outputSize = inputSize;

			var inc = Increment;
			if (position < Increment)
				inc = position + length;

			if(inc < inputSize)
			{
				Buffer.BlockCopy(inputBuffer, inc, inputBuffer, 0, inputSize - inc);

				var read = InnerStream.Read(inputBuffer, inputSize - inc, inc);

				inputSize += read;
				inputSize -= inc;
			}
			else
			{
				inputSize = 0;
			}

			if(position < Increment)
			{
				var bytes = replaceWith == null ? new byte[0] : Encoding.GetBytes(replaceWith);

				NextReplacement = new Replacement
				                  	{
				                  		Bytes = bytes,
				                  		Position = 0
				                  	};
			}
			else
			{
				NextReplacement = null;
			}
		}

		void InitializeInputBuffer()
		{
			inputBuffer = new byte[BufferSize];
			inputSize = InnerStream.Read(inputBuffer, 0, BufferSize);
		}

		internal class Replacement
		{
			public byte[] Bytes { get; set; }
			public int Position { get; set; }
		}
	}
}