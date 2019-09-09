using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Token.Models
{
    [DynamoDBTable("User")]
    public class User
    {
        [DynamoDBHashKey]   
        public string       Id           { get; set; }

        [DynamoDBProperty]
        public List<Player> Players      { get; set; }

        [DynamoDBProperty]
        public string       PasswordHash { get; set; }
    }
}