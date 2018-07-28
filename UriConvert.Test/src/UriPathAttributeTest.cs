using System;
using NUnit.Framework;

namespace Ycode.UriConvert
{
    [TestFixture]
    public class UriPathAttributeTest
    {
        class ValidUriModel0
        {
            public string Base { get; set; } = "http://example.com";
            [UriPath]
            public string UriPath { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestValidInput0()
        {
            var uriData = new ValidUriModel0();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Param0=test0"));
        }

        class ValidUriModel1
        {
            [UriBase]
            public string UriBase { get; set; } = "http://example.com";
            [UriPath]
            public string UriPath { get; set; } = "test/path";
            public string Path { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestValidInput1()
        {
            var uriData = new ValidUriModel1();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Path=test%2Fpath&Param0=test0"));
        }
    }
}
