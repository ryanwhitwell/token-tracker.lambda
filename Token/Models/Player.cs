using System;
using Token.Models.Interfaces;

namespace Token.Models
{
    public class Player: IItem
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string   Name        { get; set; }
        public int      Points      { get; set; }
    }
}