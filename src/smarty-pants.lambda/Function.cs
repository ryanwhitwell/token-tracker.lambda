using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Slight.Alexa.Framework.Models.Requests;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace smarty_pants.lambda
{
  public class Function
  {
    public string FunctionHandler(SkillRequest input, ILambdaContext ILambdaContext)
    {
      this.LogMessage(ILambdaContext, input);

      return "End";
    }

    public void LogMessage(ILambdaContext ctx, SkillRequest input)
    {
      ctx.Logger.LogLine(
          string.Format("{0}:{1} - {2}",
              ctx.AwsRequestId,
              ctx.FunctionName,
              JsonConvert.SerializeObject(input)));
    }
  }
}
