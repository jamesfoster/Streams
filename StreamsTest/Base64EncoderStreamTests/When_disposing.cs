namespace StreamsTest.Base64EncoderStreamTests
{
	using Machine.Specifications;

	public class When_disposing : Given_a_Base64EncoderStream
	{
		Because of = () => Stream.Dispose();

		It should_dispose_the_inner_stream = () => InnerStream.CanRead.ShouldEqual(false);
	}
}