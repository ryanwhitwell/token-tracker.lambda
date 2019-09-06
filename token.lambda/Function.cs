using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace token.lambda
{
  public class Function
  {
    public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
    {
      // build the speech response 
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>Hey {0}, this app is really awesome.</speak>", "Sara");
      
      // create the response using the ResponseBuilder
      SkillResponse finalResponse = ResponseBuilder.Tell(speech);
      return finalResponse;
    }
  }
}
