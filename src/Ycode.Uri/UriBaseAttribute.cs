using System;

namespace Ycode.UriConvert
{
    [Obsolete(UriConvert.ObsoleteMessage)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriBaseAttribute : Ycode.Uri.UriBaseAttribute
    {
    }
}

namespace Ycode.Uri
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriBaseAttribute : Attribute
    {
    }
}
