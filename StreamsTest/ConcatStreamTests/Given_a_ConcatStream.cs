namespace StreamsTest.ConcatStreamTests
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using Machine.Specifications;
	using Streams;
	using System.Linq;

	public class Given_a_ConcatStream
	{
		protected static ConcatStream Stream;
		protected static Stream[] InnerStreams;
		protected static string Result;
		protected static string ExpectedResult;

		Establish context = () =>
			{
				var input = new[] {"first", "second"};
				var encoding = Encoding.UTF8;

				InnerStreams = input
					.Select(i =>
						{
							var bytes = encoding.GetBytes(i);
							return new MemoryStream(bytes);
						})
					.ToArray();

				ExpectedResult = "firstsecond";

				Stream = new ConcatStream(InnerStreams);
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
				sb.Append(Encoding.UTF8.GetChars(buffer, 0, read));
			}

			Result = sb.ToString();
		}
	}
}