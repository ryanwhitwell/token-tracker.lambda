using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Alexa.NET.Response;

using smarty_pants.lambda;
using Alexa.NET.Request;

namespace smarty_pants.lambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestFunction()
        {
          Assert.Equal("HELLO WORLD", "HELLO WORLD");
        }
    }
}
