using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ycode.UriConvert
{
    [TestFixture]
    class UriConvertTest
    {
        class ValidUriModel0
        {
            public string Base { get; set; } = "http://example.com";
            public string Path { get; set; } = "test/path";
            public string Param0 { get; set; } = "test0";
            private string Param1 { get; set; } = "test1";
            protected string Param2 { get; set; } = "test2";
            public string Param3 = "test3";
            public List<string> Param4 { get; set; }
                = new List<string>
                {
                "test4-0",
                "test4-1"
                };
            public Dictionary<string, string> Param5 { get; set; }
                = new Dictionary<string, string>
                {
                { "test5-0", "value5-0" },
                { "test5-1", "value5-1" }
                };
        }

        [TestCase]
        public void TestValidInput0()
        {
            var uriData = new ValidUriModel0();
            var queryParams = uriData.ToUriQueryParameters();

            Assert.That(queryParams.Count, Is.EqualTo(2));
            Assert.That(queryParams.ContainsKey("Param0"));
            Assert.That(queryParams["Param0"], Is.EqualTo("test0"));
            Assert.That(queryParams.ContainsKey("Param4"));
            Assert.That(queryParams["Param4"], Is.EqualTo("test4-0,test4-1"));
        }

        [TestCase]
        public void TestToUriString()
        {
            var uriData = new ValidUriModel0();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Param0=test0&Param4=test4-0%2Ctest4-1"));
        }

        [TestCase]
        public void TestEncoding()
        {
            var uriData = new ValidUriModel0
            {
                Base = "http://example.com/",
                Path = "/test/path",
                Param0 = ";,/?:@&=+$ -_.!~*'() ABC abc #123",
                Param4 = new List<string>
                {
                    ";,/?:@&=+$",
                    "-_.!~*'()",
                    "ABC abc #123"
                }
            };
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?" +
                                        "Param0=%3B%2C%2F%3F%3A%40%26%3D%2B%24%20-_.%21~%2A%27%28%29%20ABC%20abc%20%23123" +
                                        "&Param4=%3B%2C%2F%3F%3A%40%26%3D%2B%24%2C-_.%21~%2A%27%28%29%2CABC%20abc%20%23123"));
        }

        [TestCase]
        public void TestValidInput1()
        {
            var uriData = new ValidUriModel0
            {
                Param0 = null
            };
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Param0=&Param4=test4-0%2Ctest4-1"));
        }

        [TestCase]
        public void TestValidInput2()
        {
            var uriData = new ValidUriModel0
            {
                Param4 = new List<string>()
            };
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Param0=test0&Param4="));
        }

        [TestCase]
        public void TestValidInput3()
        {
            var uriData = new ValidUriModel0
            {
                Path = null
            };
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/?Param0=test0&Param4=test4-0%2Ctest4-1"));
        }

        class ValidUriModel1
        {
            public string Base { get; set; } = "http://example.com";
            public string Param0 { get; set; } = "test0";
        }

        [TestCase]
        public void TestValidInput4()
        {
            var uriData = new ValidUriModel1();
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/?Param0=test0"));
        }

        class InvalidUriModel0
        {
            public string Param0 { get; set; } = "test0";
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestValidInput5()
        {
            var uriData = new ValidUriModel0
            {
                Param4 = null
            };
            var uri = uriData.ToUriString();

            Assert.That(uri, Is.EqualTo("http://example.com/test/path?Param0=test0&Param4="));
        }

        [TestCase]
        public void TestInvalidInput0()
        {
            ValidUriModel0 uriData = null;
            ArgumentNullException e = null;
            try
            {
                var uri = uriData.ToUri();
            }
            catch (ArgumentNullException exception)
            {
                e = exception;
            }
            Assert.That(e, Is.Not.Null);
        }

        [TestCase]
        public void TestInvalidInput1()
        {
            var uriData = new InvalidUriModel0();
            UriFormatException e = null;
            try
            {
                var uri = uriData.ToUri();
            }
            catch (UriFormatException exception)
            {
                e = exception;
            }
            Assert.That(e, Is.Not.Null);
            Assert.That(e.Message, Is.EqualTo("The object did not have Base URI."));
        }

        [TestCase]
        public void TestInvalidInput2()
        {
            var uriData = new ValidUriModel0
            {
                Base = null
            };
            UriFormatException e = null;
            try
            {
                var uri = uriData.ToUri();
            }
            catch (UriFormatException exception)
            {
                e = exception;
            }
            Assert.That(e, Is.Not.Null);
            Assert.That(e.Message, Is.EqualTo("The object's Base URI Property Base was null."));
        }
    }
}
