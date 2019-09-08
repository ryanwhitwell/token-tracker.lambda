using System;

namespace Token.Models.Interfaces
{
    public interface IItem
    {
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate{ get; set; }
    }
}