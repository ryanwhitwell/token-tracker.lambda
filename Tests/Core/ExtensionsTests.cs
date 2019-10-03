using Xunit;
using Token.Core;
using System.Collections.Generic;
using Token.Models;
using System;
using System.Linq;
using Alexa.NET;
using Alexa.NET.Response;

namespace Token.Tests.Core
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

      TokenUser cleansedTokenUser = tokenUser.Clean();

      Assert.Equal(tokenUser.Id, cleansedTokenUser.Id);
      Assert.Equal(tokenUser.CreateDate, cleansedTokenUser.CreateDate);
      Assert.Equal(tokenUser.UpdateDate, cleansedTokenUser.UpdateDate);
      Assert.Equal(tokenUser.HasPointsPersistence, cleansedTokenUser.HasPointsPersistence);
      Assert.Equal(tokenUser.ExpirationDate, cleansedTokenUser.ExpirationDate);
      Assert.True(tokenUser.Players.SequenceEqual(cleansedTokenUser.Players));

      Assert.NotEqual(tokenUser.PasswordHash, cleansedTokenUser.PasswordHash);
    }

    [Fact]
    public void Tell_ShouldReturnSkillResponse_WhenInputIsValid()
    {
      string phrase = "This is a test";
      SkillResponse response = phrase.Tell();

      string expectedSpeechText = string.Format("<speak>{0}</speak>", phrase);

      SsmlOutputSpeech outputSpeech = response.Response.OutputSpeech as SsmlOutputSpeech;

      Assert.IsType<SkillResponse>(response);
      Assert.IsType<SsmlOutputSpeech>(response.Response.OutputSpeech);
      Console.WriteLine(response);
      Assert.Equal(expectedSpeechText, outputSpeech.Ssml);
    }
  }
}
