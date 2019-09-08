namespace Tests
{
  public static class TestAssets
  {
    public const string CodePage1252Characters = ":€**‚ƒ„…†‡ˆ‰Š‹Œ**Ž****‘’“”•–—˜™š›œ**žŸ!\"¡#¢$£%¤&¥'¦(§)¨*©+ª,«-¬./®0¯1°2±3²4³5´6µ7¶8·9¸¹;º<»=¼>½?¾@¿AÀBÁCÂDÃEÄFÅGÆHÇIÈJÉKÊLËMÌNÍOÎPÏQÐRÑSÒTÓUÔVÕWÖX×YØZÙ[Ú\\Û]Ü^Ý_Þ`ßaàbácâdãeäfågæhçièjékêlëmìníoîpïqðrñsòtóuôvõwöx÷yøzù{ú|û}ü~ý";

    public const string AllPrintableAsciiCharacters = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

    // In Regex expressions this is [0-9]
    public const string AllNumericAsciiCharacters = "0123456789";

    // In Regex expressions this is [A-Z] or sometimes Lu
    public const string AllUppercaseAsciiCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // In Regex expressions this is [a-z] or sometimes Ll
    public const string AllLowercaseAsciiCharacters = "abcdefghijklmnopqrstuvwxyz";
  }
}
