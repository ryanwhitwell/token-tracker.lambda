using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Token.Core.StringExtensions;

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

        public bool HasPointsPersistence { get; set; }

        public static TokenUser GetCleansedTokenUser(TokenUser tokenUser)
        {
            return new TokenUser()
            {
                Id                   = tokenUser.Id,
                CreateDate           = tokenUser.CreateDate,
                UpdateDate           = tokenUser.UpdateDate,
                Players              = tokenUser.Players,
                PasswordHash         = tokenUser.PasswordHash != null ? tokenUser.PasswordHash.Mask() : null,
                HasPointsPersistence = tokenUser.HasPointsPersistence,
                TTL                  = tokenUser.TTL
            };
        }
    }
}