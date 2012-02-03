namespace StreamsTest.FluentTests
{
	using System.IO;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_Substream
	{
		static MemoryStream innerStream;
		static Substream substream;

		Establish context = () =>
			{
				innerStream = new MemoryStream();
			};

		Because of = () => { substream = innerStream.Substream(3, 6); };

		It should_not_be_null = () => substream.ShouldNotBeNull();
		It should_contain_the_inner_stream = () => substream.InnerStream.ShouldBeTheSameAs(innerStream);
		It should_start_at_the_correct_position = () => substream.StartPosition.ShouldEqual(3);
		It should_be_the_correct_length = () => substream.Length.ShouldEqual(6);
	}
}