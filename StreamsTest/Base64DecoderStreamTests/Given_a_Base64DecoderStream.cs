namespace StreamsTest.Base64DecoderStreamTests
{
	using System;
	using System.IO;
	using System.Text;
	using Machine.Specifications;
	using Streams;

	public class Given_a_Base64DecoderStream
	{
		protected static Base64DecoderStream Stream;
		protected static string Result;
		protected static string ExpectedResult;

		Establish context = () =>
			{
				ExpectedResult = "This is a test";
				var encoding = Encoding.UTF8;
				var bytes = encoding.GetBytes(ExpectedResult);
				var encodedString = Convert.ToBase64String(bytes);
				var encodedBytes = Encoding.ASCII.GetBytes(encodedString);

				var memoryStream = new MemoryStream(encodedBytes);
				Stream = new Base64DecoderStream(memoryStream);
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