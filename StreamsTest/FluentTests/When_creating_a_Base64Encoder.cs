namespace StreamsTest.FluentTests
{
	using System.IO;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_Base64Encoder
	{
		static MemoryStream innerStream;
		static Base64EncoderStream base64Encoder;

		Establish context = () =>
			{
				innerStream = new MemoryStream();
			};

		Because of = () => { base64Encoder = innerStream.Base64Encoder(); };

		It should_not_be_null = () => base64Encoder.ShouldNotBeNull();
		It should_contain_the_inner_stream = () => base64Encoder.InnerStream.ShouldBeTheSameAs(innerStream);
	}
}