Y-code URI Convert
==================

This is a serializer that converts POCO to URI.

Documentation
-------------
For the details of how to use, see
https://github.com/y-code/uri-convert/wiki

Source Code
-----------
All the source code is open to the public at GitHub.
Visit https://github.com/y-code/uri-convert

Example
-------
Have your URI in the form of a class like

class UriModel
{
    [UriBase]
    public string Endpoint { get; set; } = "http://example.com";
    [UriPath]
    public string Method { get; set; } = "test/path";
    public string Param0 { get; set; } = "test:0";
    [UriQueryParameter("arg1")]
    public string Param1 { get; set; } = "test:1";
}

then UriConvert.SerializeObject(new UriData()) can generate

http://example.com/test/path?Param0=test%3A0&arg1=test%3A1

This also can be called using an extension method; new UriData().ToUriString().
