using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Token
{
  public class Function
  {
    public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
    {
      // build the speech response 
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>Hey {0}, this app is really awesome.</speak>", "Sara");
      
      // create the response using the ResponseBuilder
      SkillResponse finalResponse = ResponseBuilder.Tell(speech);

      string tableName = "token";
      int itemsCount = await this.GetAllItemsCount(tableName);

      Console.WriteLine("Found {0} item(s) in the table '{1}'.", itemsCount, tableName);

      return finalResponse;
    }

    public async Task<int> GetAllItemsCount(string tableName) 
    {
      AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
      ScanRequest req = new ScanRequest(tableName);
      ScanResponse resp = await client.ScanAsync(req);
      return resp.Count;
    }
  }
}
