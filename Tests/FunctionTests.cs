using System;
using Token;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Alexa.NET.Response;
using Alexa.NET.Request;

namespace Tests
{
    public class FunctionTests
    {
        [Fact]
        public void TestFunction()
        {
            Assert.Equal("HELLO WORLD", "HELLO WORLD");
        }
    }
}
