namespace StreamsTest.Base64EncoderStreamTests
{
	using System;
	using System.IO;
	using System.Text;
	using Machine.Specifications;
	using Streams;

	public class Given_a_Base64EncoderStream
	{
		protected static Base64EncoderStream Stream;
		protected static string Result;
		protected static string ExpectedResult;

		Establish context = () =>
			{
				var input = "This is a test";
				var encoding = Encoding.UTF8;
				var bytes = encoding.GetBytes(input);

				ExpectedResult = Convert.ToBase64String(bytes);

				var memoryStream = new MemoryStream(bytes);
				Stream = new Base64EncoderStream(memoryStream);
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