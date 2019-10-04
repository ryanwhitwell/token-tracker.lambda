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

      ResponseBody responseBody = new ResponseBody();
      responseBody.OutputSpeech = speech;
      responseBody.ShouldEndSession = true;

      SkillResponse skillResponse = new SkillResponse();
      skillResponse.Response = responseBody;
      skillResponse.Version = "1.0";

      return skillResponse;
    }

    public static SkillResponse TellWithReprompt(this string phrase, string repromptPhrase)
    {
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>{0}</speak>", phrase);

      PlainTextOutputSpeech repromptMessage = new PlainTextOutputSpeech();
      repromptMessage.Text = repromptPhrase;

      Reprompt repromptBody = new Reprompt();
      repromptBody.OutputSpeech = repromptMessage;

      ResponseBody responseBody = new ResponseBody();
      responseBody.OutputSpeech = speech;
      responseBody.ShouldEndSession = false;
      responseBody.Reprompt = repromptBody;

      SkillResponse skillResponse = new SkillResponse();
      skillResponse.Response = responseBody;
      skillResponse.Version = "1.0";

      return skillResponse;
    }

    public static SkillResponse TellWithCard(this string phrase, ICard card)
    {
      PlainTextOutputSpeech plainText = new PlainTextOutputSpeech();
      plainText.Text = string.Format("{0}", phrase);

      ResponseBody responseBody = new ResponseBody();
      responseBody.OutputSpeech = plainText;
      responseBody.ShouldEndSession = null;
      responseBody.Card = card;

      SkillResponse skillResponse = new SkillResponse();
      skillResponse.Response = responseBody;
      skillResponse.Version = "1.0";

      return skillResponse;
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

    public static double TTLSecondsRemaining(this TokenUser tokenUser)
    {
      int configurableTtlMinutes = int.Parse(Configuration.File.GetSection("Application")["DataTimeToLiveMinutes"]);

      double configurableTtlSeconds = configurableTtlMinutes * 60;
      
      long now = (long)(DateTime.UtcNow - EPOCH_DATE).TotalSeconds;
      
      double secondsLeft = tokenUser.TTL == null ? configurableTtlSeconds : tokenUser.TTL.Value - now;

      return secondsLeft;
    }

    public static string TTLPhrase(this TokenUser tokenUser)
    {
      double secondsLeft = tokenUser.TTLSecondsRemaining();
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