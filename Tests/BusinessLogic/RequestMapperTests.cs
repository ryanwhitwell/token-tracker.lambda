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
  public class RequestMapperTests
  {
    internal class UnknownRequest : Request { }

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
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<RequestMapper>> mockLogger                = new Mock<ILogger<RequestMapper>>();
      List<Mock<IRequestRouter>>   mockRequestRouters        = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      Assert.IsType<RequestMapper>(sut);
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Mock<ILogger<RequestMapper>> mockLogger                = new Mock<ILogger<RequestMapper>>();
      List<Mock<IRequestRouter>>   mockRequestRouters        = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      Assert.Throws<ArgumentNullException>(() => new RequestMapper(null, mockLogger.Object, mockRequestRouters.Select(x => x.Object)));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenRequestRoutersIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<RequestMapper>> mockLogger                = new Mock<ILogger<RequestMapper>>();

      Assert.Throws<ArgumentNullException>(() => new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, null));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenRequestRoutersIsEmpty()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<RequestMapper>> mockLogger                = new Mock<ILogger<RequestMapper>>();
      List<Mock<IRequestRouter>>   mockRequestRouters        = new List<Mock<IRequestRouter>>();

      Assert.Throws<ArgumentNullException>(() => new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object)));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      List<Mock<IRequestRouter>>   mockRequestRouters        = new List<Mock<IRequestRouter>>();

      Assert.Throws<ArgumentNullException>(() => new RequestMapper(mockSkillRequestValidator.Object, null, mockRequestRouters.Select(x => x.Object)));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowArgumentNullException_WhenSkillRequestIsInvalid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);
      Mock<ILogger<RequestMapper>> mockLogger                = new Mock<ILogger<RequestMapper>>();
      List<Mock<IRequestRouter>>   mockRequestRouters        = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(new Mock<IRequestRouter>());

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      Assert.Throws<ArgumentNullException>(() => sut.GetRequestHandler(new SkillRequest()));
    }

    [Fact]
    public void GetRequestHandler_ShouldReturnIntentRequestHandler_WhenSkillRequestTypeIsIntentRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      IRequestRouter intentRequestHandler = sut.GetRequestHandler(GenerateValidSkillRequest(new IntentRequest() { RequestId= "TestRequestId", Type = "IntentRequest", Locale = "en-US"}));
      
      Assert.Equal(RequestType.IntentRequest, intentRequestHandler.RequestType);
    }

    [Fact]
    public void GetRequestHandler_ShouldReturnConnectionResponseRequestHandler_WhenSkillRequestTypeIsConnectionResponseRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.ConnectionResponseRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      IRequestRouter requestHandler = sut.GetRequestHandler(GenerateValidSkillRequest(new ConnectionResponseRequest() { RequestId= "TestRequestId", Type = "Connections.Response", Locale = "en-US"}));
      
      Assert.True(requestHandler != null);
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsAccountLinkSkillEventRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new AccountLinkSkillEventRequest() { RequestId= "TestRequestId", Type = "AccountLinkSkillEventRequest", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsAudioPlayerRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new AudioPlayerRequest(){ RequestId= "TestRequestId", Type = "AudioRequestType.PlaybackStarted", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsDisplayElementSelectedRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new DisplayElementSelectedRequest(){ RequestId= "TestRequestId", Type = "DisplayElementSelectedRequest", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldReturnLaunchRequestHandler_WhenSkillRequestTypeIsLaunchRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.LaunchRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      IRequestRouter intentRequestHandler = sut.GetRequestHandler(GenerateValidSkillRequest(new LaunchRequest() { RequestId= "TestRequestId", Type = "LaunchRequest", Locale = "en-US"}));
      
      Assert.Equal(RequestType.LaunchRequest, intentRequestHandler.RequestType);
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsPermissionSkillEventRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new PermissionSkillEventRequest(){ RequestId= "TestRequestId", Type = "PermissionSkillEventRequest", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsPlaybackControllerRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new PlaybackControllerRequest(){ RequestId= "TestRequestId", Type = "PlaybackControllerRequestType.Next", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldReturnSessionEndedRequestHandler_WhenSkillRequestTypeIsSessionEndedRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.SessionEndedRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      IRequestRouter intentRequestHandler = sut.GetRequestHandler(GenerateValidSkillRequest(new SessionEndedRequest() { RequestId= "TestRequestId", Type = "SessionEndedRequest", Locale = "en-US"}));
      
      Assert.Equal(RequestType.SessionEndedRequest, intentRequestHandler.RequestType);
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsSkillEventRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new SkillEventRequest(){ RequestId= "TestRequestId", Type = "SkillEventRequest", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsSystemExceptionRequest()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));
      
      Assert.Throws<NotSupportedException>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new SystemExceptionRequest(){ RequestId= "TestRequestId", Type = "SkillEventRequest", Locale = "en-US"})));
    }

    [Fact]
    public void GetRequestHandler_ShouldThrowNotSupportedException_WhenSkillRequestTypeIsUnknown()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<ILogger<RequestMapper>> mockLogger = new Mock<ILogger<RequestMapper>>();

      Mock<IRequestRouter> mockRequestrouter = new Mock<IRequestRouter>();
      mockRequestrouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      List<Mock<IRequestRouter>> mockRequestRouters = new List<Mock<IRequestRouter>>();
      mockRequestRouters.Add(mockRequestrouter);

      RequestMapper sut = new RequestMapper(mockSkillRequestValidator.Object, mockLogger.Object, mockRequestRouters.Select(x => x.Object));

      Assert.Throws<Exception>(() => sut.GetRequestHandler(GenerateValidSkillRequest(new UnknownRequest())));
    }
  }
}