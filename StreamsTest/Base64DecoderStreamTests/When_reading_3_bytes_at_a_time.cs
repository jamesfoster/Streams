namespace StreamsTest.Base64DecoderStreamTests
{
	using Machine.Specifications;

	public class When_reading_3_bytes_at_a_time : Given_a_Base64DecoderStream
	{
		Because of = () => ReadAll(3);

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}