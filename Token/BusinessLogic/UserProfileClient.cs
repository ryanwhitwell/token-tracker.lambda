using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using Token.BusinessLogic.Interfaces;
using Token.Models;

namespace Token.BusinessLogic
{
  public class UserProfileClient : IUserProfileClient
  {
    static readonly HttpClient client = new HttpClient();

    private ILogger<UserProfileClient> logger;
    
    public UserProfileClient(ILogger<UserProfileClient> logger) 
    {
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      this.logger = logger;
    }
    
    public async virtual Task<string> GetUserId(string accessToken)
    {
      Logger logger = LogManager.GetCurrentClassLogger();
      
      try	
      {
        string requestUri = string.Format("https://api.amazon.com/user/profile?access_token={0}", accessToken);
        HttpResponseMessage response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        
        logger.Debug("Response Body: " + responseBody);

        UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(responseBody);

        return profile.Id;
      }  
      catch(HttpRequestException e)
      {
        logger.Error(e);
        throw e;
      }
    }
  }
}