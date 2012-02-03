namespace StreamsTest.SubstreamTests
{
	using System.IO;
	using Machine.Specifications;

	public class When_extracting_from_a_stream : Given_a_Substream
	{
		Because of = () =>
			{
				var streamReader = new StreamReader(Stream);
				Result = streamReader.ReadToEnd();
			};

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}
