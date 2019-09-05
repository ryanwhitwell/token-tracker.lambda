using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using System;
using System.Threading.Tasks;

namespace smarty_pants.lambda
{
  public class Function
  {
    /// <summary>
    /// The main entry point for the custom runtime.
    /// </summary>
    /// <param name="args"></param>
    private static async Task Main(string[] args)
    {
      Func<SkillRequest, ILambdaContext, SkillResponse> func = FunctionHandler;
      using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new JsonSerializer()))
      using (var bootstrap = new LambdaBootstrap(handlerWrapper))
      {
        await bootstrap.RunAsync();
      }
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    ///
    /// To use this handler to respond to an AWS event, reference the appropriate package from 
    /// https://github.com/aws/aws-lambda-dotnet#events
    /// and change the string input parameter to the desired event type.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
    {
      // build the speech response 
      var speech = new SsmlOutputSpeech();
      speech.Ssml = "<speak>Today is <say-as interpret-as=\"date\">????0922</say-as>.<break strength=\"x-strong\"/>I hope you have a good day.</speak>";

      // create the response using the ResponseBuilder
      var finalResponse = ResponseBuilder.Tell(speech);
      return finalResponse;
    }
  }
}
