using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Token.Core;

namespace Token.Models
{
  [DynamoDBTable("TokenUser")]
  public class TokenUser
  {
    [DynamoDBHashKey]
    public string Id { get; set; }

    [DynamoDBProperty]
    public DateTime? CreateDate { get; set; }

    [DynamoDBProperty]
    public DateTime? UpdateDate { get; set; }

    [DynamoDBProperty]
    public List<Player> Players { get; set; }

    [DynamoDBProperty]
    public string PasswordHash { get; set; }

    [DynamoDBProperty]
    public long? TTL { get; set; }

    [DynamoDBProperty]
    public int UpsellTicks { get; set; }

    public bool HasPointsPersistence { get; set; }

    public int DataTtlMinutes
    {
      get 
      {
        int configuredTtlMinutes = int.Parse(Configuration.File.GetSection("Application")["DataTimeToLiveMinutes"]);
        DateTime userCreateDate = this.CreateDate.HasValue ? this.CreateDate.Value : DateTime.UtcNow;
        DateTime userDataExpirationDate = userCreateDate.AddMinutes(configuredTtlMinutes);
        int userDataTtlMinutes = (int)(userDataExpirationDate - userCreateDate).TotalMinutes;

        return userDataTtlMinutes;
      }
    }
  }
}