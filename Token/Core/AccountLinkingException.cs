using System;

namespace Token.Core.Exceptions
{
  public class AccountLinkingException : Exception
  {
    public AccountLinkingException()
    {
    }

    public AccountLinkingException(string message)
        : base(message)
    {
    }

    public AccountLinkingException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}