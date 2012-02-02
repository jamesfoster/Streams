## Intro

Streams is a collection of useful streams for encoding, decoding, scanning or manipulating other streams.

## Examples

In most cases these streams are wrappers around one or more other streams. The wrapper will also dispose its inner streams when disposed. There are also extension methods on `Stream` to make it easy to chain them together.

#### Base64EncoderStream / Base64DecoderStream

```c#
var output = new FileStream("output.txt", FileMode.OpenOrCreate, FileAccess.Write)
var input = new FileStream("input.txt", FileMode.Open, FileAccess.Read)

using(output)
using(var base64Encoder = input.Base64Encoder())
{
  base64Encoder.CopyTo(output);
}
```

In this example we read from `input.txt`, convert it to base64 using the `Base64EncoderStream` and write the output to `output.txt`. Notice `input` is not directly disposed inside a `using()` statement but it will be disposed when `base64Encoder` is disposed.

The `Base64DecoderStream` works in exactly the same way except it expects the input to be properly formatted base64. By default it will ignore any whitespace in the input stream but you can override that to be more strict. If the input is not valid base64 an Exception will be thrown when you attempt to read.

```c#
using(var base64Decoder = input.Base64Decoder(Base64DecodeMode.DoNotIgnoreWhiteSpaces))
{
  base64Decoder.CopyTo(output);
}
```

#### RegexFindReplaceStream

The `RegexFindReplaceStream` can be used to find occurances of a regular expression and replace them with the result of an evaluator method.

```c#
var replacements = new Dictionary<Regex, Func<Match, string>>
  {
    {new Regex("_([^_\n\r]+)_"), m => string.Format("<b>{0}</b>", m.Groups[1].Value)},
    {new Regex("abc"), m => "xyz"}
  }
var maxMatchLength = 1024;

using(var replaceStream = input.RegexFindReplace(replacements, maxMatchLength, Encoding.UTF8)
{
  replaceStream.CopyTo(output);
}
```

In this example we scan the input for occurances of 2 regular expressions. When it finds an occurance it calls the matching lambda expression passing in the `Match`.

**Note:** the `maxMatchLength` is specified as 1024 bytes. Any matching occurance under this length will successfully be captured. You may, however, still receive longer occurances (up to `maxMatchLength * 2`).

Limitations: 

* *Matches can not overlap.*

	For example if the input contained `_abc_` the first regex would match and return `<b>abc</b>` and scanning would continue from the end of that match. In order to get the output of `<b>xyz</b>` you would need to wrap one `RegexFindReplaceStream` inside another.

* *The order of the patterns matters.*

	If 2 patterns match at the same position, the first in the list of replacements succeeds.