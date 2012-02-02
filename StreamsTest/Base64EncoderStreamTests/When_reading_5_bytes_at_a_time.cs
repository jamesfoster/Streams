namespace StreamsTest.Base64EncoderStreamTests
{
	using Machine.Specifications;

	public class When_reading_5_bytes_at_a_time : Given_a_Base64EncoderStream
	{
		Because of = () => ReadAll(5);

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}