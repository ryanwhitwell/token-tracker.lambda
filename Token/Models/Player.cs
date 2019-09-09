using System;
using Amazon.DynamoDBv2.DataModel;

namespace Token.Models
{
    public class Player
    {
        [DynamoDBProperty]
        public string   Name        { get; set; }
        
        [DynamoDBProperty]
        public int      Points      { get; set; }
    }
}