namespace StreamsTest.FindReplaceStreamTests
{
	using Machine.Specifications;

	public class When_disposing : Given_a_FindReplaceStream
	{
		Because of = () => Stream.Dispose();

		It should_dispose_the_inner_stream = () => InnerStream.CanRead.ShouldEqual(false);
	}
}