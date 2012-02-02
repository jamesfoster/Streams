namespace StreamsTest.FluentTests
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_RegexFindReplace2
	{
		static MemoryStream innerStream;
		static RegexFindReplaceStream regexFindReplace;
		static Dictionary<Regex, string> replacements;
		static int maxMatchLength;
		static Encoding encoding;

		Establish context = () =>
			{
				innerStream = new MemoryStream();
				replacements = new Dictionary<Regex, string>
				               	{
				               		{new Regex("abc"), "xyz"},
				               		{new Regex("def"), "uvw"}
				               	};
				maxMatchLength = 1234;
				encoding = Encoding.UTF8;
			};

		Because of = () => { regexFindReplace = innerStream.RegexFindReplace(replacements, maxMatchLength, encoding); };

		It should_not_be_null = () => regexFindReplace.ShouldNotBeNull();
		It should_contain_the_inner_stream = () => regexFindReplace.InnerStream.ShouldBeTheSameAs(innerStream);
		It should_have_correct_maxMatchLength = () => regexFindReplace.MaxMatchLength.ShouldEqual(maxMatchLength);
		It should_have_correct_Encoding = () => regexFindReplace.Encoding.ShouldEqual(encoding);

		It should_contain_1_replacement = () =>
			regexFindReplace.Replacements.Count.ShouldEqual(2);

		It replacement_1_should_have_correct_Regex = () =>
			regexFindReplace.Replacements.ElementAt(0).Key.ToString().ShouldEqual("abc");

		It replacement_2_should_have_correct_Regex = () =>
			regexFindReplace.Replacements.ElementAt(1).Key.ToString().ShouldEqual("def");
	}
}