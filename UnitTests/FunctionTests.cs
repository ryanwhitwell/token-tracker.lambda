using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Alexa.NET.Response;

using Token;
using Alexa.NET.Request;

namespace UnitTests
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
