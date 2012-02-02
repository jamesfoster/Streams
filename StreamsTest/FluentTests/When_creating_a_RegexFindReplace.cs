namespace StreamsTest.FluentTests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using Machine.Specifications;
	using Streams;

	public class When_creating_a_RegexFindReplace
	{
		static MemoryStream innerStream;
		static RegexFindReplaceStream regexFindReplace;
		static Dictionary<Regex, Func<Match, string>> replacements;
		static int maxMatchLength;
		static Encoding encoding;

		Establish context = () =>
			{
				innerStream = new MemoryStream();
				replacements = new Dictionary<Regex, Func<Match, string>>
				               	{
				               		{new Regex("abc"), m => "xyz"},
				               		{new Regex("def"), m => "uvw"}
				               	};
				maxMatchLength = 1234;
				encoding = Encoding.UTF8;
			};

		Because of = () => { regexFindReplace = innerStream.RegexFindReplace(replacements, maxMatchLength, encoding); };

		It should_not_be_null = () => regexFindReplace.ShouldNotBeNull();
		It should_contain_the_inner_stream = () => regexFindReplace.InnerStream.ShouldBeTheSameAs(innerStream);
		It should_contain_the_specified_replacements = () => regexFindReplace.Replacements.ShouldBeTheSameAs(replacements);
		It should_have_correct_maxMatchLength = () => regexFindReplace.MaxMatchLength.ShouldEqual(maxMatchLength);
		It should_have_correct_Encoding = () => regexFindReplace.Encoding.ShouldEqual(encoding);
	}
}