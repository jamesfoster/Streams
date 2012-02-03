namespace StreamsTest.FluentTests
{
	using System.IO;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_ConcatStream
	{
		static MemoryStream innerStream1;
		static MemoryStream innerStream2;
		static MemoryStream innerStream3;
		static ConcatStream concatStream;

		Establish context = () =>
			{
				innerStream1 = new MemoryStream();
				innerStream2 = new MemoryStream();
				innerStream3 = new MemoryStream();
			};

		Because of = () => { concatStream = innerStream1.Concat(innerStream2, innerStream3); };

		It should_not_be_null = () => concatStream.ShouldNotBeNull();
		It should_contain_the_inner_stream1 = () => concatStream.InnerStreams[0].ShouldBeTheSameAs(innerStream1);
		It should_contain_the_inner_stream2 = () => concatStream.InnerStreams[1].ShouldBeTheSameAs(innerStream2);
		It should_contain_the_inner_stream3 = () => concatStream.InnerStreams[2].ShouldBeTheSameAs(innerStream3);
	}
}