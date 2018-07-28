using System;
using NUnit.Framework;

namespace Ycode.UriConvert
{
    [TestFixture(TestOf = typeof(UriQueryParameterAttribute))]
    public class UriQueryParameterAttributeTest
    {
        class DummyUriModel
        {
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename()
        {
            var uriData = new DummyUriModel();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);
        }

        class DummyUriModel1
        {
            public string Base { get; set; }
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename1()
        {
            var uriData = new DummyUriModel1();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Base"), Is.False);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);
        }

        class DummyUriModel2
        {
            public string Path { get; set; }
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename2()
        {
            var uriData = new DummyUriModel2();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Path"), Is.False);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);
        }

        class DummyUriModel3
        {
            [UriQueryParameter("Base")]
            public string Base { get; set; }
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename3()
        {
            var uriData = new DummyUriModel3();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Base"), Is.True);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);
        }

        class DummyUriModel4
        {
            [UriQueryParameter("Path")]
            public string Path { get; set; }
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename4()
        {
            var uriData = new DummyUriModel4();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Path"), Is.True);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);
        }

        class DummyUriModel5
        {
            [UriBase]
            [UriQueryParameter("uriBase")]
            public string UriBase { get; set; } = "https://example.com";
            [UriPath]
            [UriQueryParameter("uriPath")]
            public string UriPath { get; set; } = "/test/path";
            public string Param0 { get; set; } = "test0";
            [UriQueryParameter("parameter1")]
            public string Param1 { get; set; } = "test1";
        }

        [TestCase]
        public void TestQueryParameterRename5()
        {
            var uriData = new DummyUriModel5();
            var parameters = UriConvert.ExtractQueryParameters(uriData);

            Assert.That(parameters.ContainsKey("Base"), Is.False);
            Assert.That(parameters.ContainsKey("UriBase"), Is.False);
            Assert.That(parameters.ContainsKey("uriBase"), Is.True);
            Assert.That(parameters.ContainsKey("Path"), Is.False);
            Assert.That(parameters.ContainsKey("UriPath"), Is.False);
            Assert.That(parameters.ContainsKey("uriPath"), Is.True);
            Assert.That(parameters.ContainsKey("Param1"), Is.False);
            Assert.That(parameters.ContainsKey("parameter1"), Is.True);

            var uri = uriData.ToUriString();
            Assert.That(uri, Is.EqualTo("https://example.com/test/path?uriBase=https%3A%2F%2Fexample.com&uriPath=%2Ftest%2Fpath&Param0=test0&parameter1=test1"));
        }

        class DummyUriModel6
        {
            public string Base { get; set; } = "http://example.com";
            public string Path { get; set; } = "test/path";
            [UriQueryParameter("Param1")]
            public string Param0 { get; set; } = "test0";
            public string Param1 { get; set; } = "test1";
            [UriQueryParameter("Param3")]
            public string Param2 { get; set; } = "test2";
            public string Param3 { get; set; } = "test3";
            public string Param4 { get; set; } = "test4";
        }

        [TestCase]
        public void TestQueryParameterRename6()
        {
            var uriData = new DummyUriModel6();

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
            Assert.That(e.GetType(), Is.EqualTo(typeof(InvalidOperationException)));
            Assert.That(e.Message, Is.EqualTo("There are duplicates in query parameters in type Ycode.UriConvert.UriQueryParameterAttributeTest+DummyUriModel6." +
                                              " Query parameter name \"Param1\" is used by property Param0 and Param1." +
                                              " Query parameter name \"Param3\" is used by property Param2 and Param3."));
        }
    }
}
