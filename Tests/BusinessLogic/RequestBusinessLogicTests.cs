using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Token.BusinessLogic;
using Token.BusinessLogic.Interfaces;
using Token.Data.Interfaces;
using Xunit;
using System.Linq;
using System;
using Token.Models;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.InSkillPricing;

namespace Token.Tests.BusinessLogic
{
  public class RequestBusinessLogicTests
  {
    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenInputIsValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      Assert.IsType<RequestBusinessLogic>(sut);
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillProductsAdapterIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockSkillRequestValidator.Object, null, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(null, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, null, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenInputRequestRoutersIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, null, mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenRequestRoutersIsEmpty()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenTokenDataIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GenerateEmptyTokenUser_ShouldThrowArgumentNullException_WhenIdIsNotProvided(string id)
    {
      Assert.Throws<ArgumentNullException>(() => RequestBusinessLogic.GenerateEmptyTokenUser(id));
    }

    [Fact]
    public void GenerateEmptyTokenUser_ShouldReturnATokenUser_WhenIdIsProvided()
    {
      string id = "abc123";

      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = id,
        Players = new List<Player>()
      };

      TokenUser tokenUser = RequestBusinessLogic.GenerateEmptyTokenUser(id);

      Assert.IsType<TokenUser>(tokenUser);
      Assert.Equal(expectedTokenUser.Id, tokenUser.Id);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnTrue_WhenUserHasPointsPersistenceAndIsEntitled()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.Entitled, ProductId = "amzn1.adg.product.467f7ca4-91dd-48d3-b831-040673e7066c" }}
        }
      ));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.True(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasPointsPersistenceAndIsNotEntitled()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.NotEntitled, ProductId = "amzn1.adg.product.467f7ca4-91dd-48d3-b831-040673e7066c" }}
        }
      ));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserDoesNotHavePointsPersistence()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.NotEntitled, ProductId = "NotPointsPersistence" }}
        }
      ));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserDoesNotHaveAnySkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = null
        }
      ));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasEmptySkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(null));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldThrowArgumentNullException_WhenKillRequestIsNotValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(null));

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HasPointsPersistence(skillRequest));
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasPointsPersistenceCannotReadTheUsersSkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ISkillProductsClient> mockInSkillProductsClient = new Mock<ISkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Throws(new Exception());

      Mock<ISkillProductsAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            User = new User()
            {
              UserId = "TestUserId"
            }
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId"
        }
      };

      RequestBusinessLogic sut = new RequestBusinessLogic(mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object), mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.False(hashPointsPersistence);
    }
  }
}
