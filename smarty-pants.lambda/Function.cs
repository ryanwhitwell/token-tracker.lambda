using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace smarty_pants.lambda
{
  public class Function
  {
    public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
    {
      // build the speech response 
      var speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>Today is <say-as interpret-as=\"date\">{0}</say-as>.<break strength=\"x-strong\"/>I hope you have a good day.</speak>", DateTime.Now.ToShortDateString());

      // create the response using the ResponseBuilder
      var finalResponse = ResponseBuilder.Tell(speech);
      return finalResponse;
    }
  }
}
