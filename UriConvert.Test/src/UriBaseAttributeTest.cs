using System;
using NUnit.Framework;

namespace Ycode.UriConvert
{
    [TestFixture]
    public class UriBaseAttributeTest
    {
        class ValidUriModel0
        {
            [UriBase]
            public string UriBase { get; set; } = "http://example.com";
            public string Path { get; set; } = "test/path";
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
            public string Base { get; set; } = "http://example.com";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestValidInput1()
        {
            var uriData = new ValidUriModel1();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Base=http%3A%2F%2Fexample.com&Param0=test0"));
        }

        class InvalidUriModel0
        {
            [UriBase]
            [UriIgnore]
            public string UriBase { get; set; } = "http://example.com";
            [UriPath]
            public string UriPath { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestInvalidInput2()
        {
            var uriData = new InvalidUriModel0();

            Exception e = null;
            try
            {
                var uri = uriData.ToUriString();
            }
            catch (Exception exception)
            {
                e = exception;
            }

            Assert.That(e, Is.Not.Null);
            Assert.That(e.GetType(), Is.EqualTo(typeof(UriFormatException)));
            Assert.That(e.Message, Is.EqualTo("The object did not have Base URI."));
        }
    }
}
