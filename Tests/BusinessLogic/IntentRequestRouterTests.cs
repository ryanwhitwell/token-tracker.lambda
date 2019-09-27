using Token.BusinessLogic;
using Xunit;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Token.BusinessLogic.Interfaces;
using System.Linq;
using System;
using Token.Core;
using Alexa.NET.InSkillPricing.Responses;

namespace Token.Tests.BusinessLogic
{
  public class IntentRequestRouterTests
  {
    private static SkillRequest GenerateValidSkillRequest(Request request)
    { 
      SkillRequest skillRequest = new SkillRequest()
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
        Request = request
      };

      return skillRequest;
    }

    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenDependenciesAreValid()
    {
      Mock<ISkillRequestValidator>       mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<IntentRequestRouter>> mockLogger                = new Mock<ILogger<IntentRequestRouter>>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      Assert.IsType<IntentRequestRouter>(sut);
    }

    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenSkillRequestValidatorIsNull()
    {
      Mock<ILogger<IntentRequestRouter>> mockLogger                = new Mock<ILogger<IntentRequestRouter>>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());

      Assert.Throws<ArgumentNullException>(() => new IntentRequestRouter(null, mockLogger.Object, mockRequestHandlers.Select(x => x.Object)));
    }

    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator>       mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());

      Assert.Throws<ArgumentNullException>(() => new IntentRequestRouter(mockSkillRequestValidator.Object, null, mockRequestHandlers.Select(x => x.Object)));
    }

    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenRequestHandlersIsNull()
    {
      Mock<ISkillRequestValidator>       mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<IntentRequestRouter>> mockLogger                = new Mock<ILogger<IntentRequestRouter>>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());


      Assert.Throws<ArgumentNullException>(() => new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, null));
    }
  }
}