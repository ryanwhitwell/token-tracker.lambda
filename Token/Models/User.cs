using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Token.Models.Interfaces;

namespace Token.Models
{
    [DynamoDBTable("User")]
    public class User: IItem
    {
        public DateTime     CreatedDate  { get; set; }
        public DateTime     UpdatedDate  { get; set; }
        
        [DynamoDBHashKey]   
        public string       Id           { get; set; }

        public List<Player> Players      { get; set; }

        public string       PasswordHash { get; set; }
    }
}