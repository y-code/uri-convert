using System;

namespace Ycode.UriConvert
{
    [Obsolete(UriConvert.ObsoleteMessage)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriIgnoreAttribute : Ycode.Uri.UriIgnoreAttribute
    {
    }
}

namespace Ycode.Uri
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriIgnoreAttribute : Attribute
    {
    }
}
