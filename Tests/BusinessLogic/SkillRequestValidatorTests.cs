using Token.BusinessLogic;
using Xunit;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;

namespace Token.Tests.BusinessLogic
{
  public class SkillRequestvalidatorTests
  {
    [Fact]
    public void IsValid_ReturnsTrue_WhenSkillRequestIsValid()
    {
      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            ApiEndpoint = "http://localhost",
            ApiAccessToken = "TestApiAccessToken",
            User = new User()
            {
            UserId = "TestUserId", 
AccessToken = "TestAccessToken"
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
      
      SkillRequestValidator sut = new SkillRequestValidator();

      Assert.True(sut.IsValid(skillRequest));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenSkillRequestIsNull()
    {
      SkillRequestValidator sut = new SkillRequestValidator();

      Assert.False(sut.IsValid(null));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenSkillRequestIsInValid_MissingUserId()
    {
      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            ApiEndpoint = "TestApiEndpoint",
            ApiAccessToken = "TestApiAccessToken",
            User = new User()
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId",
          Type = "IntentRequest"
        }
      };
      
      SkillRequestValidator sut = new SkillRequestValidator();

      Assert.False(sut.IsValid(skillRequest));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenSkillRequestIsInValid_MissingUser()
    {
      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            ApiEndpoint = "TestApiEndpoint",
            ApiAccessToken = "TestApiAccessToken"
          }
        },
        Request = new IntentRequest()
        {
          RequestId = "TestRequestId",
          Type = "IntentRequest"
        }
      };
      
      SkillRequestValidator sut = new SkillRequestValidator();

      Assert.False(sut.IsValid(skillRequest));
    }
  }
}