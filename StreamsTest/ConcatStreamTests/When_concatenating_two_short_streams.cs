namespace StreamsTest.ConcatStreamTests
{
	using System.IO;
	using Machine.Specifications;

	public class When_concatenating_two_short_streams : Given_a_ConcatStream
	{
		Because of = () =>
			{
				var streamReader = new StreamReader(Stream);
				Result = streamReader.ReadToEnd();
			};

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}
