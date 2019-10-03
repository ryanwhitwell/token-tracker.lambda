using System;
using System.Collections.Generic;
using System.Linq;
using Token.Models;
using Xunit;

namespace Token.Tests.Models
{
  public class TokenUserTests
  {
    [Fact]
    public void Constructor_ReturnsInstanceOfClass()
    {
      string expectedId = "TestId";
      DateTime expectedCreateDate = DateTime.Now;
      DateTime expectedUpdateDate = DateTime.Now;
      DateTime expectedExpirationDate = DateTime.Now;
      string expectedPasswordHash = "TestPasswordHash";
      bool expectedHasPointsPersistence = false;
      List<Player> expectedPlayers = new List<Player>() { new Player() { Name = "TestPlayerName", Points = 3 } };

      TokenUser tokenUser = new TokenUser();
      tokenUser.Id = expectedId;
      tokenUser.CreateDate = expectedCreateDate;
      tokenUser.UpdateDate = expectedUpdateDate;
      tokenUser.PasswordHash = expectedPasswordHash;
      tokenUser.HasPointsPersistence = expectedHasPointsPersistence;
      tokenUser.ExpirationDate = expectedExpirationDate;
      tokenUser.Players = expectedPlayers;

      Assert.IsType<TokenUser>(tokenUser);
      Assert.Equal(tokenUser.Id, expectedId);
      Assert.Equal(tokenUser.CreateDate, expectedCreateDate);
      Assert.Equal(tokenUser.UpdateDate, expectedUpdateDate);
      Assert.Equal(tokenUser.PasswordHash, expectedPasswordHash);
      Assert.Equal(tokenUser.HasPointsPersistence, expectedHasPointsPersistence);
      Assert.Equal(tokenUser.ExpirationDate, expectedExpirationDate);
      Assert.True(tokenUser.Players.SequenceEqual(expectedPlayers));
    }
  }
}