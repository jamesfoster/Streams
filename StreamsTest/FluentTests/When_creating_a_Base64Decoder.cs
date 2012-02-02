namespace StreamsTest.FluentTests
{
	using System.IO;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_Base64Decoder
	{
		static MemoryStream innerStream;
		static Base64DecoderStream base64Decoder;

		Establish context = () =>
			{
				innerStream = new MemoryStream();
			};

		Because of = () => { base64Decoder = innerStream.Base64Decoder(); };

		It should_not_be_null = () => base64Decoder.ShouldNotBeNull();
		It should_contain_the_inner_stream = () => base64Decoder.InnerStream.ShouldBeTheSameAs(innerStream);
		It should_ignore_whitespaces = () => base64Decoder.Whitespaces.ShouldEqual(Base64DecodeMode.IgnoreWhiteSpaces);
	}
}