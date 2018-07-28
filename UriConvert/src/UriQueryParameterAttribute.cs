using System;

namespace Ycode.UriConvert
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UriQueryParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public UriQueryParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
