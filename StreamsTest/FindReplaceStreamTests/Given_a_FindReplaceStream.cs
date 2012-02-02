namespace StreamsTest.FindReplaceStreamTests
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using Machine.Specifications;
	using Streams;

	public class Given_a_FindReplaceStream
	{
		protected static RegexFindReplaceStream Stream;
		protected static MemoryStream InnerStream;
		protected static string Result;
		protected static string ExpectedResult;

		Establish context = () =>
			{
				var input = "ceci n'est pas une Pipe";
				var encoding = Encoding.UTF8;
				var bytes = encoding.GetBytes(input);

				InnerStream = new MemoryStream(bytes);
				var replacements = new Dictionary<string, string>
				                   	{
				                   		{"(c)e\\1i", "this"},
				                   		{"n'est", "is"},
				                   		{"[asp]{3}", "not"},
				                   		{"une", "a"},
				                   		{"(?i)pipe", "pipe"}
				                   	};

				ExpectedResult = "this is not a pipe";

				Stream = new RegexFindReplaceStream(InnerStream, replacements, 8, encoding);
			};

		Cleanup cleanup = () => Stream.Dispose();

		public static void ReadAll(int bufferSize)
		{
			var buffer = new byte[bufferSize];
			var sb = new StringBuilder();

			while (true)
			{
				var read = Stream.Read(buffer, 0, buffer.Length);
				if (read == 0)
					break;
				sb.Append(Encoding.ASCII.GetChars(buffer, 0, read));
			}

			Result = sb.ToString();
		}
	}
}