[![Build status](https://ci.appveyor.com/api/projects/status/aqpcl9xyi3d15kc8?svg=true)](https://ci.appveyor.com/project/y-code/uri-convert)
# URI Convert
URI Convert is a utility library for serializing POCO to URI string or Uri instance. If you need to dynamically generates URI, this utility can help you to make your code simple.

## Example
Have your URI in the form of a class like
```csharp
class UriModel
{
    public string Base { get; set; } = "http://example.com";
    public string Path { get; set; } = "test/path";
    public string Param0 { get; set; } = "test:0";
    public string Param1 { get; set; } = "test:1";
}
```
then `new UriModel().ToUriString()` can generate
```
http://example.com/test/path?Param0=test%3A0&Param1=test%3A1
```

## Advanced Example
This utility also provides attributes that let you name porperties more flexibly.
```csharp
class UriModel
{
    [UriBase]
    public string Endpoint { get; set; } = "http://example.com";
    [UriPath]
    public string Method { get; set; } = "test/path";
    [UriQueryParameter("arg0")]
    public string Param0 { get; set; } = "test:0";
    [UriQueryParameter("arg1")]
    public string Param1 { get; set; } = "test:1";
}
```
This is serialized to
```
http://example.com/test/path?arg0=test%3A0&arg1=test%3A1
```

## Enhanced Example
UriIgnore attribute allow you to also have non-URI-related properties together. For example, this lets you have request contents together. In case the request content is JSON, the data class can be like
```csharp
class UriModel
{
    [UriBase]
    [JsonIgnore]
    public string Endpoint { get; set; } = "http://example.com";
    [UriPath]
    [JsonIgnore]
    public string Method { get; set; } = "test/path";
    [UriQueryParameter("arg0")]
    [JsonIgnore]
    public string Param0 { get; set; } = "test0";
    [JsonProperty("arg1")]
    [UriIgnore]
    public string Param1 { get; set; } = "test1";
}
```
then URI and request content can be generated from a single object in the way that
```csharp
var uriData = new UriModel();
var uri = UriConvert.SerializeObject(uriData);
var content = JsonConvert.SerializeObject(uriData);
```
which are to be
```
http://example.com/test/path?arg0=test0
```
```json
{"arg1":"test1"}
```
