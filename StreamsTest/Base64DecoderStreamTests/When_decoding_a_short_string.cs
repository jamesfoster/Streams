namespace StreamsTest.Base64DecoderStreamTests
{
	using System.IO;
	using Machine.Specifications;

	public class When_decoding_a_short_string : Given_a_Base64DecoderStream
	{
		Because of = () =>
			{
				var streamReader = new StreamReader(Stream);
				Result = streamReader.ReadToEnd();
			};

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}
