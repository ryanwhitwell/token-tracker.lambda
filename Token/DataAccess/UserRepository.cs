using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Token.DataAccess.Interfaces;
using Token.Models;

namespace Token.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private static readonly string TABLE_NAME = "User";
        private ILogger<UserRepository> logger;
        public UserRepository(ILogger<UserRepository> logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            this.logger = logger;
        }
        
        private static RegionEndpoint CLIENT_REGION_ENDPOINT = RegionEndpoint.USEast1;
        private static AmazonDynamoDBClient CLIENT = new AmazonDynamoDBClient(CLIENT_REGION_ENDPOINT);
        private static DynamoDBContext CONTEXT = new DynamoDBContext(CLIENT);

        public async Task Save(User user)
        {
            await CONTEXT.SaveAsync<User>(user);
        }

        public async Task Delete(string id)
        {
            await CONTEXT.DeleteAsync<User>(id);
        }

        public async Task<User> Load(string id)
        {
            User user = await CONTEXT.LoadAsync<User>(id, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

            return user;
        }

        public async Task<int> GetAllItemsCount()
        {
            ScanRequest req = new ScanRequest(TABLE_NAME);
            ScanResponse resp = await CLIENT.ScanAsync(req);
            return resp.Count;
        }

        public string TableName
        {
            get
            {
                return TABLE_NAME;
            }
        }
    }
}