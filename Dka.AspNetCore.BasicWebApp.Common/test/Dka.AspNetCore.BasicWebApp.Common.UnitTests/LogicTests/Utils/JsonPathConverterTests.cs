using System;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Utils;
using Newtonsoft.Json;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.LogicTests.Utils
{
    public class JsonPathConverterTests
    {
        private const int SampleInt = 111;
        private const string SampleString = "sample string";

        [Fact]
        public void Test_Deserialize_JsonPropertyDefined_ShouldPass()
        {
            var jsonAsString = @"
                {
                    ""customProperty1"": ""111"",
                    ""path"": {
                        ""to"": {
                            ""property2"": ""sample string""
                        },
                    }
                }
            ";

            var testClass1 = JsonConvert.DeserializeObject<TestClass1>(jsonAsString);

            Assert.Equal(SampleInt, testClass1.Property1);
            Assert.Equal(SampleString, testClass1.Property2);
        }
        
        [Fact]
        public void Test_Deserialize_JsonPropertyDefined_SomePathNotExistInJsonString_ShouldPass()
        {
            var jsonAsString = @"
                {
                    ""customProperty1"": ""111"",
                }
            ";

            var testClass1 = JsonConvert.DeserializeObject<TestClass1>(jsonAsString);

            Assert.Equal(SampleInt, testClass1.Property1);
            Assert.Null(testClass1.Property2);
        }
        
        [Fact]
        public void Test_Deserialize_JsonPropertyDefined_JsonStringEmpty_ShouldPass()
        {
            var jsonAsString = @"
                {

                }
            ";

            var testClass1 = JsonConvert.DeserializeObject<TestClass1>(jsonAsString);

            Assert.Equal(default(long), testClass1.Property1);
            Assert.Null(testClass1.Property2);
        }        
        
        [Fact]
        public void Test_Deserialize_JsonPropertyDefined_JsonStringNull_ShouldPass()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                JsonConvert.DeserializeObject<TestClass1>(null);
            });
        }         
        
        [Fact]
        public void Test_Deserialize_JsonPropertyNotDefinedAndNotExistsInJsonString_ShouldPass()
        {
            var jsonAsString = @"
                {
                    ""customProperty1"": ""111"",
                    ""path"": {
                        ""to"": {
                            ""property2"": ""sample string""
                        },
                    }
                }
            ";

            var testClass2 = JsonConvert.DeserializeObject<TestClass2>(jsonAsString);

            Assert.Equal(default(long), testClass2.Property1);
            Assert.Null(testClass2.Property2);
        }

        [Fact]
        public void Test_Deserialize_JsonPropertyNotDefinedAndExistsInJsonString_ShouldPass()
        {
            var jsonAsString = @"
                {
                    ""Property1"": ""111"",
                    ""Property2"": ""sample string"",
                }
            ";

            var testClass2 = JsonConvert.DeserializeObject<TestClass2>(jsonAsString);

            Assert.Equal(SampleInt, testClass2.Property1);
            Assert.Equal(SampleString, testClass2.Property2);
        }

        [Fact]
        public void Test_CanWrite_CanConvert_ShouldPass()
        {
            var converter = new JsonPathConverter(); 
            
            Assert.False(converter.CanWrite);
            Assert.False(converter.CanConvert(typeof(object)));
        }

        [Fact]
        public void Test_WriteJson_ThrowsException_ShouldPass()
        {
            var converter = new JsonPathConverter();

            Assert.Throws<NotImplementedException>(() =>
            {
                converter.WriteJson(null, null, null);
            });
        }

        [JsonConverter(typeof(JsonPathConverter))]
        private class TestClass1
        {
            [JsonProperty(PropertyName = "customProperty1")]
            public long Property1 { get; set; }

            [JsonProperty(PropertyName = "path.to.property2")]
            public string Property2 { get; set; }
        }
        
        [JsonConverter(typeof(JsonPathConverter))]
        private class TestClass2
        {
            public long Property1 { get; set; }

            public string Property2 { get; set; }
        }        
    }
}