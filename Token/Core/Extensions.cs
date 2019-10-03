using System;
using Alexa.NET;
using Alexa.NET.Response;
using Token.Models;

namespace Token.Core
{
  public static class Extensions
  {
    public static readonly DateTime EPOCH_DATE = new DateTime(1970, 1, 1);
    
    public static SkillResponse Tell(this string phrase)
    {
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>{0}</speak>", phrase);
      SkillResponse speechResponse = ResponseBuilder.Tell(speech);
      return speechResponse;
    }

    public static SkillResponse TellWithReprompt(this string phrase, string repromptPhrase)
    {
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>{0}</speak>", phrase);

      Reprompt reprompt = new Reprompt(repromptPhrase);

      SkillResponse speechResponse = ResponseBuilder.Ask(speech, reprompt);
      return speechResponse;
    }

    public static string Mask(this string source)
    {
      return source.Mask('*');
    }

    public static string Mask(this string source, char maskCharacter)
    {
      int sourceLength = source.Length;
      int start = (int)Math.Ceiling((sourceLength / 2d) / 2d);
      int maskLength = sourceLength - start;

      string mask = new string(maskCharacter, maskLength);
      string unMaskStart = source.Substring(0, start);

      return unMaskStart + mask;
    }

    public static TokenUser Clean(this TokenUser tokenUser)
    {
      return new TokenUser()
      {
        Id = tokenUser.Id,
        CreateDate = tokenUser.CreateDate,
        UpdateDate = tokenUser.UpdateDate,
        Players = tokenUser.Players,
        PasswordHash = tokenUser.PasswordHash != null ? tokenUser.PasswordHash.Mask() : null,
        HasPointsPersistence = tokenUser.HasPointsPersistence,
        TTL = tokenUser.TTL
      };
    }

    public static string TTLPhrase(this TokenUser tokenUser)
    {
      long now = (long)(DateTime.UtcNow - EPOCH_DATE).TotalSeconds;
      
      double secondsLeft = (tokenUser.TTL.Value - now);
      double minutesLeft = Math.Ceiling(secondsLeft / 60);

      if (secondsLeft > 60)
      {
        return string.Format("for {0} more minutes", minutesLeft);
      }
      else if (secondsLeft == 60)
      {
        return string.Format("for {0} more minute", minutesLeft);
      }
      else if (secondsLeft < 60 && secondsLeft > 0)
      {
        if (secondsLeft == 1)
        {
          return string.Format("for {0} more second", secondsLeft);
        }

        return string.Format("for {0} more seconds", secondsLeft);
      }
      
      return "momentarily";
    }
  }
}