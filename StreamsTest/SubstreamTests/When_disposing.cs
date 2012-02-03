namespace StreamsTest.SubstreamTests
{
	using Machine.Specifications;

	public class When_disposing : Given_a_Substream
	{
		Because of = () => Stream.Dispose();

		It should_dispose_the_inner_streams = () => InnerStream.CanRead.ShouldEqual(false);
	}
}