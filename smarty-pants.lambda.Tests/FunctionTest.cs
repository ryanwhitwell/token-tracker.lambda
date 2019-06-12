using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using smarty_pants.lambda;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Requests.RequestTypes;

namespace smarty_pants.lambda.Tests
{
  public class FunctionTest
  {
    [Fact]
    public void TestFunction()
    {
      Function function = new Function();
      TestLambdaContext context = new TestLambdaContext();
      SkillRequest input = new SkillRequest();
      Intent myIntent = new Intent();
      myIntent.Name = "CreateRelationship";
      myIntent.Slots = new Dictionary<string, Slot>()
      {
        ["slot1"] = new Slot() { Name = "slot1Name", Value = "slot1Value" }
      };

      RequestBundle myRequestBundle = new RequestBundle();
      myRequestBundle.Type = "myType";
      myRequestBundle.RequestId = "myRequestId";
      myRequestBundle.Timestamp = new DateTime();
      myRequestBundle.Intent = myIntent;
      myRequestBundle.Reason = "myReason";
      input.Request = myRequestBundle;

      String expectedResponse = "End";
      String response = function.FunctionHandler(input, context);

      Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public void Truthtest()
    {
      Assert.True(true);
    }
  }
}
