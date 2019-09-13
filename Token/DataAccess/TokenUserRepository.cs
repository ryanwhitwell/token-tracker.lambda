using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Token.DataAccess.Interfaces;
using Token.Models;

namespace Token.DataAccess
{
  public class TokenUserRepository : ITokenUserRepository
  {
    private static RegionEndpoint CLIENT_REGION_ENDPOINT = RegionEndpoint.USEast1;
    private static AmazonDynamoDBClient CLIENT = new AmazonDynamoDBClient(CLIENT_REGION_ENDPOINT);
    private static DynamoDBContext CONTEXT = new DynamoDBContext(CLIENT, new DynamoDBContextConfig() { ConsistentRead = true });

    public async Task Save(TokenUser user)
    {
      await CONTEXT.SaveAsync<TokenUser>(user);
    }

    public async Task Delete(string id)
    {
      await CONTEXT.DeleteAsync<TokenUser>(id);
    }

    public async Task<TokenUser> Load(string id)
    {
      TokenUser user = await CONTEXT.LoadAsync<TokenUser>(id);

      return user;
    }
  }
}