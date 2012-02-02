namespace Streams
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	public static class Fluent
	{
		public static Base64DecoderStream Base64Decoder(this Stream stream,
		                                                Base64DecodeMode whitespaces = Base64DecodeMode.IgnoreWhiteSpaces)
		{
			return new Base64DecoderStream(stream, whitespaces);
		}

		public static Base64EncoderStream Base64Encoder(this Stream stream)
		{
			return new Base64EncoderStream(stream);
		}

		public static RegexFindReplaceStream RegexFindReplace(this Stream stream,
		                                                      IDictionary<string, string> replacements,
		                                                      int maxMatchLength = 4096,
		                                                      Encoding encoding = null)
		{
			if (encoding == null) encoding = Encoding.UTF8;

			return new RegexFindReplaceStream(stream, replacements, maxMatchLength, encoding);
		}

		public static RegexFindReplaceStream RegexFindReplace(this Stream stream,
		                                                      IDictionary<Regex, string> replacements,
		                                                      int maxMatchLength = 4096,
		                                                      Encoding encoding = null)
		{
			if (encoding == null) encoding = Encoding.UTF8;

			return new RegexFindReplaceStream(stream, replacements, maxMatchLength, encoding);
		}

		public static RegexFindReplaceStream RegexFindReplace(this Stream stream,
		                                                      IDictionary<Regex, Func<Match, string>> replacements,
		                                                      int maxMatchLength = 4096,
		                                                      Encoding encoding = null)
		{
			if (encoding == null) encoding = Encoding.UTF8;

			return new RegexFindReplaceStream(stream, replacements, maxMatchLength, encoding);
		}
	}
}