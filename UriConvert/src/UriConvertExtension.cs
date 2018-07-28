using System.Collections.Generic;

namespace Ycode.UriConvert
{
    public static class UriConvertExtension
    {
        public static System.Uri ToUri(this object value)
            => UriConvert.Convert(value);

        public static string ToUriString(this object value)
            => UriConvert.SerializeObject(value);

        public static IDictionary<string, string> ToUriQueryParameters(this object value, bool isToEncode = false)
            => UriConvert.ExtractQueryParameters(value, isToEncode);
    }
}
