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
using System.Threading.Tasks;
using Token.Models;
using Alexa.NET.Response;

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
             UserId = "TestUserId", 
AccessToken = "TestAccessToken"
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

    [Fact]
    public async Task GetSkillResponse_ShouldReturnSkillResponse_WhenSkillRequestIsInvalid()
    {
      Mock<ISkillRequestValidator>       mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<IntentRequestRouter>> mockLogger                = new Mock<ILogger<IntentRequestRouter>>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      SkillRequest skillRequest = GenerateValidSkillRequest(null);
      TokenUser tokenUser = new TokenUser();

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSkillResponse(skillRequest, tokenUser));
    }

    [Fact]
    public async Task GetSkillResponse_ShouldReturnSkillResponse_WhenTokenUserIsInvalid()
    {
      Mock<ISkillRequestValidator>       mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<IntentRequestRouter>> mockLogger                = new Mock<ILogger<IntentRequestRouter>>();
      List<Mock<IIntentRequestHandler>>  mockRequestHandlers       = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(new Mock<IIntentRequestHandler>());

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest() { RequestId ="TestRequestId", Locale = "en-US", Type = "IntentRequest" });

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSkillResponse(skillRequest, null));
    }

    [Fact]
    public async Task GetSkillResponse_ShouldReturnSkillResponse_WhenInputsAreInvalid()
    {
      SkillResponse expectedSkillResponse = new SkillResponse();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<IntentRequestRouter>> mockLogger = new Mock<ILogger<IntentRequestRouter>>();
      
      Mock<IIntentRequestHandler> mockIntentRequestHandler = new Mock<IIntentRequestHandler>();
      mockIntentRequestHandler.Setup(x => x.HandlerName).Returns(IntentRequestName.AddPoints);
      mockIntentRequestHandler.Setup(x => x.Handle(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).Returns(expectedSkillResponse);
      
      List<Mock<IIntentRequestHandler>> mockRequestHandlers = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(mockIntentRequestHandler);

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest() { RequestId ="TestRequestId", Locale = "en-US", Type = "IntentRequest", Intent = new Intent() { ConfirmationStatus = "CONFIRMED", Name = "AddPoints" } });
      TokenUser tokenUser = new TokenUser();

      SkillResponse skillResponse = await sut.GetSkillResponse(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public async Task GetSkillResponse_ShouldReturnSkillResponse_WhenIntentConfirmationStatusIsDenied()
    {
      SkillResponse expectedSkillResponse = string.Format("Okay").Tell(true);
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<IntentRequestRouter>> mockLogger = new Mock<ILogger<IntentRequestRouter>>();
      
      Mock<IIntentRequestHandler> mockIntentRequestHandler = new Mock<IIntentRequestHandler>();
      mockIntentRequestHandler.Setup(x => x.HandlerName).Returns(IntentRequestName.AddPoints);
      mockIntentRequestHandler.Setup(x => x.Handle(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).Returns(expectedSkillResponse);
      
      List<Mock<IIntentRequestHandler>> mockRequestHandlers = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(mockIntentRequestHandler);

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest() { RequestId ="TestRequestId", Locale = "en-US", Type = "IntentRequest", Intent = new Intent() { ConfirmationStatus = "DENIED", Name = "AddPoints" } });
      TokenUser tokenUser = new TokenUser();

      SkillResponse skillResponse = await sut.GetSkillResponse(skillRequest, tokenUser);
      SsmlOutputSpeech speechResponse = skillResponse.Response.OutputSpeech as SsmlOutputSpeech;

      Assert.IsType<SkillResponse>(skillResponse);
      Assert.IsType<SsmlOutputSpeech>(skillResponse.Response.OutputSpeech);
      Assert.Equal("<speak>Okay</speak>", speechResponse.Ssml);
    }
    
    [Fact]
    public async Task GetSkillResponse_ShouldThrowNotSupportedException_WhenIntentNameIsUnknown()
    {
      SkillResponse expectedSkillResponse = string.Format("Okay").Tell(true);
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<IntentRequestRouter>> mockLogger = new Mock<ILogger<IntentRequestRouter>>();
      
      Mock<IIntentRequestHandler> mockIntentRequestHandler = new Mock<IIntentRequestHandler>();
      mockIntentRequestHandler.Setup(x => x.HandlerName).Returns(IntentRequestName.AddPoints);
      mockIntentRequestHandler.Setup(x => x.Handle(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).Returns(expectedSkillResponse);
      
      List<Mock<IIntentRequestHandler>> mockRequestHandlers = new List<Mock<IIntentRequestHandler>>();
      mockRequestHandlers.Add(mockIntentRequestHandler);

      IntentRequestRouter sut = new IntentRequestRouter(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestHandlers.Select(x => x.Object));
      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest() { RequestId ="TestRequestId", Locale = "en-US", Type = "IntentRequest", Intent = new Intent() { ConfirmationStatus = "CONFIRMED", Name = "UnknownIntentName" } });
      TokenUser tokenUser = new TokenUser();

      await Assert.ThrowsAsync<NotSupportedException>(() => sut.GetSkillResponse(skillRequest, tokenUser));
    }
  }
}