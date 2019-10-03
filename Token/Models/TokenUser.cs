using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

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
    public DateTime? ExpirationDate { get; set; }

    [DynamoDBProperty]
    public List<Player> Players { get; set; }

    [DynamoDBProperty]
    public string PasswordHash { get; set; }

    public bool HasPointsPersistence { get; set; }
  }
}