using System;
using Alexa.NET.Request;

namespace Token.BusinessLogic
{
  public class SkillRequestValidator : ISkillRequestValidator
  {
    public bool IsValid(SkillRequest skillRequest)
    {
      if (skillRequest == null
        || skillRequest.Context == null
        || skillRequest.Context.System == null
        || String.IsNullOrWhiteSpace(skillRequest.Context.System.ApiEndpoint)
        || String.IsNullOrWhiteSpace(skillRequest.Context.System.ApiAccessToken)
        || skillRequest.Request == null
        || String.IsNullOrWhiteSpace(skillRequest.Request.RequestId)
        || String.IsNullOrWhiteSpace(skillRequest.Request.Type)
        || String.IsNullOrWhiteSpace(skillRequest.Request.Locale)
        || skillRequest.Context.System.User == null
        || String.IsNullOrWhiteSpace(skillRequest.Context.System.User.AccessToken))
       
      {
        return false;
      }

      return true;
    }
  }
}