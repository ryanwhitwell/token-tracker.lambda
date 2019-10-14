using Token.BusinessLogic;
using Xunit;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Token.Models;
using Alexa.NET.Response;
using Token.BusinessLogic.IntentRequestHandlers;

namespace Token.Tests.BusinessLogic.IntentRequestHandlers
{
  public class AddAllPointsTests
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
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      Assert.IsType<AddAllPoints>(sut);
    }

    [Fact]  
    public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      Assert.Throws<ArgumentNullException>(() => new AddAllPoints(null, mockSkillRequestValidator.Object));
    }

    [Fact]  
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      Assert.Throws<ArgumentNullException>(() => new AddAllPoints(mockLogger.Object, null));
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenTokenUserPlayersIsNull()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "2",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });
      
      TokenUser tokenUser = new TokenUser()
      {
        Players = new List<Player>()
      };

      SkillResponse skillResponse = sut.Handle(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenTokenUserPlayersIsEmpty()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "2",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });
      
      TokenUser tokenUser = new TokenUser();
      tokenUser.Players = null;

      SkillResponse skillResponse = sut.Handle(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenTokenUserPlayersHasPlayers()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "2",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });
      
      TokenUser tokenUser = new TokenUser();
      tokenUser.Players = new List<Player>() { new Player() { Name = "Blue", Points = 3 } };

      SkillResponse skillResponse = sut.Handle(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenTokenUserPlayersHasPlayersWithOnePoint()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "1",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });
      
      TokenUser tokenUser = new TokenUser();
      tokenUser.Players = new List<Player>() { new Player() { Name = "Blue", Points = 3 } };

      SkillResponse skillResponse = sut.Handle(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenTokenUserIsNull()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "1",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });
      
      Assert.Throws<ArgumentNullException>(() => sut.Handle(skillRequest, null));
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenSkillRequestIsInvalid()
    {
      Mock<ILogger<AddAllPoints>> mockLogger = new Mock<ILogger<AddAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);
      
      AddAllPoints sut = new AddAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "AddPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "amount", 
              new Slot()
              {
                Name = "amount",
                Value = "1",
                ConfirmationStatus = "NONE"
              }
            },
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });

      TokenUser tokenUser = new TokenUser();
      tokenUser.Players = new List<Player>() { new Player() { Name = "Blue", Points = 3 } };
      
      Assert.Throws<ArgumentNullException>(() => sut.Handle(skillRequest, tokenUser));
    }
  }
}