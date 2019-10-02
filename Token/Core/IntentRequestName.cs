namespace Token.Core
{
  public static class IntentRequestName
  {
    public const string GetAllPlayersCount = "GetAllPlayersCount";
    public const string AddPlayer          = "AddPlayer";
    public const string AddPoints          = "AddPoints";
    public const string RemovePoints       = "RemovePoints";
    public const string AddAllPoints       = "AddAllPoints";
    public const string RemoveAllPoints    = "RemoveAllPoints";
    public const string AddSinglePoint     = "AddSinglePoint";
    public const string RemoveSinglePoint  = "RemoveSinglePoint";
    public const string ResetAllPoints     = "ResetAllPoints";
    public const string ListAllPlayers     = "ListAllPlayers";
    public const string ListAllPoints      = "ListAllPoints";
    public const string GetPlayerPoints    = "GetPlayerPoints";
    public const string GetPointsMax       = "GetPointsMax";
    public const string GetPointsMin       = "GetPointsMin";
    public const string GetPointsAverage   = "GetPointsAverage";
    public const string DeletePlayer       = "DeletePlayer";
    public const string DeleteAllPlayers   = "DeleteAllPlayers";
    public const string Buy                = "Buy";
    public const string WhatCanIBuy        = "WhatCanIBuy";
    public const string Help               = "AMAZON.HelpIntent";
    public const string RefundSubscription = "RefundSubscription";
    public const string Fallback           = "AMAZON.FallbackIntent";
    public const string Cancel             = "AMAZON.CancelIntent";
    public const string Stop               = "AMAZON.StopIntent";
    public const string NavigateHome       = "AMAZON.NavigateHomeIntent";
  }
}