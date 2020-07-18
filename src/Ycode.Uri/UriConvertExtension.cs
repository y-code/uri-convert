using System;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete
namespace Ycode.UriConvert
{
    public static class UriConvertExtension
    {
        [Obsolete(UriConvert.ObsoleteMessage)]
        public static System.Uri ToUri(this object value)
            => UriConvert.Convert(value);

        [Obsolete(UriConvert.ObsoleteMessage)]
        public static string ToUriString(this object value)
            => UriConvert.SerializeObject(value);

        [Obsolete(UriConvert.ObsoleteMessage)]
        public static IDictionary<string, string> ToUriQueryParameters(this object value, bool isToEncode = false)
            => UriConvert.ExtractQueryParameters(value, isToEncode);
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

namespace Ycode.Uri
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
