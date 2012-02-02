namespace StreamsTest.FindReplaceStreamTests
{
	using System.IO;
	using Machine.Specifications;

	public class When_scanning_a_short_string : Given_a_FindReplaceStream
	{
		Because of = () =>
			{
				var streamReader = new StreamReader(Stream);
				Result = streamReader.ReadToEnd();
			};

		It should_output_the_expected_result = () => Result.ShouldEqual(ExpectedResult);
	}
}
