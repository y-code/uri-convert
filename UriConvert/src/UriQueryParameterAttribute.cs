using System;

namespace Ycode.UriConvert
{
    [Obsolete(UriConvert.ObsoleteMessage)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriQueryParameterAttribute : Ycode.Uri.UriQueryParameterAttribute
    {
        public UriQueryParameterAttribute(string name) : base(name) { }

        public UriQueryParameterAttribute() : base() { }
    }
}

namespace Ycode.Uri
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriQueryParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public UriQueryParameterAttribute(string name)
        {
            Name = name;
        }

        public UriQueryParameterAttribute() : this(null)
        {
        }
    }
}
