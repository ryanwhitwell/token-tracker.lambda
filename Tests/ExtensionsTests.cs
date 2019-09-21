using Xunit;
using Token.Core;
using System.Collections.Generic;
using Token.Models;
using System;
using System.Linq;

namespace Tests
{
  public class ExtensionsTests
  {
    [Theory]
    [InlineData("abc123", "ab****")]
    [InlineData("abc123d", "ab*****")]
    [InlineData("abc123de", "ab******")]
    [InlineData("abc123def", "abc******")]
    [InlineData("95#exDwB$x87c2ZAB^$L4", "95#exD***************")]
    public void Mask_ShouldMaskUsingTheCorrectFormat(string source, string expectedResult)
    {
      string result = source.Mask();
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Clean_ShouldNotReturnFullValueInPasswordHash()
    {
      string expectedId = "TestId";
      DateTime expectedCreateDate = DateTime.Now;
      DateTime expectedUpdateDate = DateTime.Now;
      string expectedPasswordHash = "TestPasswordHash";
      bool expectedHasPointsPersistence = false;
      long expectedTTL = 123456789;
      List<Player> expectedPlayers = new List<Player>() { new Player() { Name = "TestPlayerName", Points = 3 } };

      TokenUser tokenUser = new TokenUser();
      tokenUser.Id = expectedId;
      tokenUser.CreateDate = expectedCreateDate;
      tokenUser.UpdateDate = expectedUpdateDate;
      tokenUser.PasswordHash = expectedPasswordHash;
      tokenUser.HasPointsPersistence = expectedHasPointsPersistence;
      tokenUser.TTL = expectedTTL;
      tokenUser.Players = expectedPlayers;

      TokenUser cleansedTokenUser = tokenUser.Clean();

      Assert.Equal(tokenUser.Id, cleansedTokenUser.Id);
      Assert.Equal(tokenUser.CreateDate, cleansedTokenUser.CreateDate);
      Assert.Equal(tokenUser.UpdateDate, cleansedTokenUser.UpdateDate);
      Assert.Equal(tokenUser.HasPointsPersistence, cleansedTokenUser.HasPointsPersistence);
      Assert.Equal(tokenUser.TTL, cleansedTokenUser.TTL);
      Assert.True(tokenUser.Players.SequenceEqual(cleansedTokenUser.Players));

      Assert.NotEqual(tokenUser.PasswordHash, cleansedTokenUser.PasswordHash);
    }
  }

}
