namespace StreamsTest.SubstreamTests
{
	using System.IO;
	using System.Text;
	using Machine.Specifications;
	using Streams;

	public class Given_a_Substream
	{
		protected static Substream Stream;
		protected static MemoryStream InnerStream;
		protected static string Result;
		protected static string ExpectedResult;

		Establish context = () =>
			{
				var input = "abcdefghijklmnopqrstuvwxyz";
				var encoding = Encoding.UTF8;
				var bytes = encoding.GetBytes(input);

				InnerStream = new MemoryStream(bytes);

				ExpectedResult = "defghijklmnopqrstuvw";

				Stream = new Substream(InnerStream, 3, 20);
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