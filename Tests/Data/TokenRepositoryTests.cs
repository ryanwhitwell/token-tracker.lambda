using System;
using Xunit;
using Moq;
using Amazon.DynamoDBv2.DataModel;
using Token.Data;
using Token.Data.Interfaces;
using System.Threading.Tasks;
using Token.Models;
using System.Threading;

namespace Token.Tests.Data
{
  public class TokenRepositoryTests
  {
    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenInputIsValid()
    {
      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      ITokenUserRepository sut = new TokenUserRepository(mock.Object);
      Assert.IsType<TokenUserRepository>(sut);
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
      Assert.Throws<ArgumentNullException>(() => new TokenUserRepository(null));
    }

    [Fact]
    public async Task Save_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      ITokenUserRepository sut = new TokenUserRepository(mock.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Save(null));
    }

    [Fact]
    public async Task Save_ShouldComplete_WhenInputIsValid()
    {
      TokenUser user = new TokenUser()
      {
        Id = "abc123"
      };

      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      mock.Setup(x => x.SaveAsync<TokenUser>(It.IsAny<TokenUser>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

      ITokenUserRepository repository = new TokenUserRepository(mock.Object);
      await repository.Save(user);
      mock.Verify(x => x.SaveAsync<TokenUser>(It.IsAny<TokenUser>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Delete_ShouldThrowArgumentNullException_WhenInputIsNull(string id)
    {
      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      ITokenUserRepository sut = new TokenUserRepository(mock.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Delete(id));
    }

    [Fact]
    public async Task Delete_ShouldComplete_WhenInputIsValid()
    {
      string id = "abc123";

      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      mock.Setup(x => x.DeleteAsync<TokenUser>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

      ITokenUserRepository repository = new TokenUserRepository(mock.Object);
      await repository.Delete(id);
      mock.Verify(x => x.DeleteAsync<TokenUser>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Load_ShouldThrowArgumentNullException_WhenInputIsNull(string id)
    {
      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      ITokenUserRepository sut = new TokenUserRepository(mock.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Load(id));
    }

    [Fact]
    public async Task Load_ShouldComplete_WhenInputIsValid()
    {
      TokenUser expectedUser = new TokenUser()
      {
        Id = "abc123"
      };

      Mock<IDynamoDBContext> mock = new Mock<IDynamoDBContext>();
      mock.Setup(x => x.LoadAsync<TokenUser>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(expectedUser));

      ITokenUserRepository repository = new TokenUserRepository(mock.Object);
      TokenUser user = await repository.Load(expectedUser.Id);
      mock.Verify(x => x.LoadAsync<TokenUser>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());

      Assert.IsType<TokenUser>(user);
      Assert.Equal(expectedUser, user);
    }
  }
}