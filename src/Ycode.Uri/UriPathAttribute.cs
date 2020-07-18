using System;

namespace Ycode.UriConvert
{
    [Obsolete(UriConvert.ObsoleteMessage)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriPathAttribute : Ycode.Uri.UriPathAttribute
    {
    }
}

namespace Ycode.Uri
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriPathAttribute : Attribute
    {
    }
}
