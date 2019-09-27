using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Token.BusinessLogic;
using Token.BusinessLogic.Interfaces;
using Token.Data.Interfaces;
using Xunit;
using System;
using Token.Models;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.InSkillPricing;
using Alexa.NET.Response;
using Token.Core;
using Amazon.Lambda.Core;

namespace Token.Tests.BusinessLogic
{
  public class SkillProductsAdapterTests
  {
    private static readonly SkillRequest ValidSkillRequest = new SkillRequest()
    {
      Context = new Context()
      {
        System = new AlexaSystem()
        {
          ApiEndpoint = "http://localhost",
          ApiAccessToken = "xx508xx63817x752xx74004x30705xx92x58349x5x78f5xx34xxxxx51",
          User = new User()
          {
            UserId = "TestUserId"
          }
        }
      },
      Request = new IntentRequest()
      {
        RequestId = "TestRequestId",
        Type = "IntentRequest",
        Locale = "en-US"
      }
    };
    
    [Fact]
    public void Ctor_ShouldReturnInstanceOfAdapter_WhenDependenciesAreValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      SkillProductsClientAdapter sut = new SkillProductsClientAdapter(mockSkillRequestValidator.Object);

      Assert.IsType<SkillProductsClientAdapter>(sut);
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Assert.Throws<ArgumentNullException>(() => new SkillProductsClientAdapter(null));
    }
    
    [Fact]
    public void GetClient_ShouldReturnInstanceOfISkillProductsClient_WhenSkillRequestIsValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      SkillProductsClientAdapter sut = new SkillProductsClientAdapter(mockSkillRequestValidator.Object);
      
      ISkillProductsClient client = sut.GetClient(ValidSkillRequest);

      Assert.IsAssignableFrom<ISkillProductsClient>(client);
    }

    [Fact]
    public void GetClient_ShouldThrowArgumentNullException_WhenSkillRequestIsNotValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      SkillProductsClientAdapter sut = new SkillProductsClientAdapter(mockSkillRequestValidator.Object);
      
      Assert.Throws<ArgumentNullException>(() => sut.GetClient(new SkillRequest()));
    }
  }
}