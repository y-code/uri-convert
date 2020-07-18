using System;
using NUnit.Framework;

namespace Ycode.Uri.Test
{
    [TestFixture(TestOf = typeof(UriIgnoreAttribute))]
    public class UriIgnoreAttributeTest
    {
        class ValidUriModel0
        {
            public string Param0 { get; set; } = "test0";
            [UriIgnore]
            public string Param1 { get; set; } = "test1";
            [UriIgnore]
            [UriQueryParameter("parameter2")]
            public string Param2 { get; set; } = "test2";
        }

        [TestCase]
        public void TestIgnoredPropertyTest0()
        {
            var uriData = new ValidUriModel0();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Param0"), Is.True);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("Param2"), Is.False);
            Assert.That(parameters.ContainsKey("parameter2"), Is.False);
        }

        class ValidUriModel1
        {
            public string Base { get; set; } = "http://example.com";
            [UriIgnore]
            public string Path { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestIgnoredPropertyTest1()
        {
            var uriData = new ValidUriModel1();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/?Param0=test0"));
        }


        class ValidUriModel2
        {
            public string Base { get; set; } = "http://example.com";
            [UriPath]
            [UriIgnore]
            public string Path { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestIgnoredPropertyTest2()
        {
            var uriData = new ValidUriModel2();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/?Param0=test0"));
        }

        class InvalidUriModel0
        {
            [UriIgnore]
            public string Base { get; set; } = "http://example.com";
        }

        [TestCase]
        public void TestIgnoredPropertyTest3()
        {
            var uriData = new InvalidUriModel0();

            Exception e = null;
            try
            {
                var parameters = uriData.ToUriString();
            }
            catch (Exception exception)
            {
                e = exception;
            }
            Assert.That(e, Is.Not.Null);
            Assert.That(e.GetType(), Is.EqualTo(typeof(UriFormatException)));
            Assert.That(e.Message, Is.EqualTo("The object did not have Base URI."));
        }

        class InvalidUriModel1
        {
            [UriBase]
            [UriIgnore]
            public string Base { get; set; } = "http://example.com";
        }

        [TestCase]
        public void TestIgnoredPropertyTest4()
        {
            var uriData = new InvalidUriModel1();

            Exception e = null;
            try
            {
                var parameters = uriData.ToUriString();
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
