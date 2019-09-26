using System;
using Xunit;
using Moq;
using Amazon.DynamoDBv2.DataModel;
using Token.Data;
using Token.Data.Interfaces;
using System.Threading.Tasks;
using Token.Models;
using System.Threading;
using Microsoft.Extensions.Logging;
using Token.Core;

namespace Token.Tests.Data
{
  public class TokenUserDataTests
  {
    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenDependenciesAreProvided()
    {
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      Assert.IsType<TokenUserData>(sut);
    }

    [Fact]
    public void Ctor_ShouldReturnThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      
      Assert.Throws<ArgumentNullException>(() => new TokenUserData(null, mockTokenUserRepository.Object));
    }

    [Fact]
    public void Ctor_ShouldReturnThrowArgumentNullException_WhenTokenUserRepositoryIsNull()
    {
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      
      Assert.Throws<ArgumentNullException>(() => new TokenUserData(mockLogger.Object, null));
    }

    [Fact]
    public async Task Delete_ShouldReturn_WhenIdIsValid()
    {
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Delete(It.IsAny<string>())).Returns(Task.FromResult(true));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      await sut.Delete("TestId");

      mockTokenUserRepository.Verify(x => x.Delete("TestId"), Times.Once());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Delete_ShouldThrowArgumentNullException_WhenIdIsInvalid(string id)
    {
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Delete(It.IsAny<string>())).Returns(Task.FromResult(true));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Delete(id));
    }

    [Fact]
    public async Task Exists_ShouldReturnTrue_WhenTokenUserIsFoundAndIdsMatch()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Load(It.IsAny<string>())).Returns(Task.FromResult(expectedTokenUser));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      bool exists = await sut.Exists(expectedTokenUser.Id);

      mockTokenUserRepository.Verify(x => x.Load(expectedTokenUser.Id), Times.Once());
      Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ShouldReturnFalse_WhenTokenUserIsFoundAndIdsDoNotMatch()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };

      TokenUser unknownTokenUser = new TokenUser()
      {
        Id = "UnknownUserId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Load(It.IsAny<string>())).Returns(Task.FromResult(unknownTokenUser));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      bool exists = await sut.Exists(expectedTokenUser.Id);

      mockTokenUserRepository.Verify(x => x.Load(expectedTokenUser.Id), Times.Once());
      Assert.False(exists);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Exists_ShouldThrowArgumentNullException_WhenIdIsInvalid(string id)
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Load(It.IsAny<string>())).Returns(Task.FromResult(expectedTokenUser));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Exists(id));
    }

    [Fact]
    public async Task Get_ShouldReturnTokenUser_WhenUserExists()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Load(It.IsAny<string>())).Returns(Task.FromResult(expectedTokenUser));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      TokenUser tokenUser = await sut.Get(expectedTokenUser.Id);

      mockTokenUserRepository.Verify(x => x.Load(expectedTokenUser.Id), Times.Once());
      Assert.Equal(expectedTokenUser, tokenUser);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Get_ShouldThrowArgumentNullException_WhenIdIsInvalid(string id)
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Load(It.IsAny<string>())).Returns(Task.FromResult(expectedTokenUser));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Get(id));
    }

    [Fact]
    public async Task Save_ShouldThrowArgumentNullException_WhenTokenUserIsNull()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Save(It.IsAny<TokenUser>())).Returns(Task.FromResult(true));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Save(null));
    }

    [Fact]
    public async Task Save_ShouldReturn_WhenTokenUserUsValid()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "TestId"
      };
      
      Mock<ILogger<TokenUserData>> mockLogger = new Mock<ILogger<TokenUserData>>();
      Mock<ITokenUserRepository> mockTokenUserRepository = new Mock<ITokenUserRepository>();
      mockTokenUserRepository.Setup(x => x.Save(It.IsAny<TokenUser>())).Returns(Task.FromResult(true));
      
      TokenUserData sut = new TokenUserData(mockLogger.Object, mockTokenUserRepository.Object);
      await sut.Save(expectedTokenUser);

      mockTokenUserRepository.Verify(x => x.Save(expectedTokenUser), Times.Once());
    }

    [Fact]
    public void GetTTL_ShouldReturnCorrectValue_WhenCalledWithValidDate()
    {
      DateTime? dateTime = DateTime.UtcNow;
      DateTime epoch = new DateTime(1970, 1, 1);

      long ttlSeconds = TokenUserData.GetTTL(dateTime);
      long expectedTtlSeconds = (long)(dateTime.Value.AddMinutes(30) - epoch).TotalSeconds;

      
      Assert.Equal(expectedTtlSeconds, ttlSeconds);
    }

    [Fact]
    public void GetTTL_ShouldThrowArgumentNullException_WhenDateIsNull()
    {
      Assert.Throws<ArgumentNullException>(() => TokenUserData.GetTTL(null));
    }
  }
}