namespace StreamsTest.ConcatStreamTests
{
	using Machine.Specifications;

	public class When_disposing : Given_a_ConcatStream
	{
		Because of = () => Stream.Dispose();

		It should_dispose_the_inner_streams = () => InnerStreams.ShouldEachConformTo(s => s.CanRead == false);
	}
}