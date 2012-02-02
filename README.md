# Streams

A collection of useful streams for encoding, decoding, scanning or manipulating other streams.

# Usage

```c#
using(var magicStream = inputStream.RegexFindReplace(replacements).Base64Encode())
{
  magicStream.CopyTo(outputStream);
}
```

# Documentation #

Please visit the wiki for more [documentation](/jamesfoster/Streams/wiki/Streams).
