using System;
using Token.Core.Security;
using Xunit;

namespace Token.Tests.Core.Security
{
  public class HashUtilityTests
  {
    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldThrowAnArgumentNullException_WhenNullValueIsProvided()
    {
      string password = null;

      Assert.Throws<ArgumentNullException>(() => HashUtility.CreateHash(password));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldThrowAnArgumentNullException_WhenEmptyStringIsProvided()
    {
      string password = "";

      Assert.Throws<ArgumentNullException>(() => HashUtility.CreateHash(password));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldThrowAnArgumentNullException_WhenWhiteSpaceIsProvided()
    {
      string password = "      ";

      Assert.Throws<ArgumentNullException>(() => HashUtility.CreateHash(password));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateHash_WhenColonProvidedInPassword()
    {
      string password = "cxjknviwedsnsd:skdhfiwqle123";

      string hash = HashUtility.CreateHash(password);

      Assert.True(PasswordHashIsValid(hash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateHash_WhenSingleColonProvidedAsPassword()
    {
      string password = ":";

      string hash = HashUtility.CreateHash(password);

      Assert.True(PasswordHashIsValid(hash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateHash_WhenOnlyColonsProvidedInPassword()
    {
      string password = ":::::::::::::::::::";

      string hash = HashUtility.CreateHash(password);

      Assert.True(PasswordHashIsValid(hash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateHash_WhenUsingCodePage1252CharactersInPassword()
    {
      string password = TestAssets.CodePage1252Characters;

      string hash = HashUtility.CreateHash(password);

      Assert.True(PasswordHashIsValid(hash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateHash_WhenUsingAllPrintableAsciiCharactersInPassword()
    {
      string password = TestAssets.AllPrintableAsciiCharacters;

      string hash = HashUtility.CreateHash(password);

      Assert.True(PasswordHashIsValid(hash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void CreateHash_ShouldCreateManyHashes_WhenManyPasswordsAreProvidedInSuccession()
    {
      for (int i = 0; i < 99; i++)
      {
        string password = string.Format("Password{0}#", i);

        string hash = HashUtility.CreateHash(password);

        Assert.True(PasswordHashIsValid(hash));
      }
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordAndCorrectHashAreNull()
    {
      string password = null;

      string correctHash = null;

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordIsEmptyAndCorrectHashIsNull()
    {
      string password = "";

      string correctHash = null;

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordIsWhiteSpaceAndCorrectHashIsNull()
    {
      string password = "    ";

      string correctHash = null;

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordIsNullAndCorrectHashIsEmpty()
    {
      string password = null;

      string correctHash = "";

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordIsNullAndCorrectHashIsWhiteSpace()
    {
      string password = null;

      string correctHash = "   ";

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordAndCorrectHashAreEmpty()
    {
      string password = "";

      string correctHash = "";

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnArgumentNullException_WhenPasswordAndCorrectHashAreWhiteSpace()
    {
      string password = "   ";

      string correctHash = "   ";

      Assert.Throws<ArgumentNullException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnInvalidOperationException_WhenCorrectHashSplitLengthIsTooSmall()
    {
      string password = "Password2#";

      string correctHash = "asdjlkdsf:jkfafsdf";

      Assert.Throws<InvalidOperationException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAnInvalidOperationException_WhenCorrectHashSplitLengthIsTooLarge()
    {
      string password = "Password2#";

      string correctHash = "as:djlkdsf:jkfa:fsdf";

      Assert.Throws<InvalidOperationException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldReturnFalse_WhenCorrectHashDoesNotContainIntegerAsFirstElement()
    {
      string password = "Password2#";

      string correctHash = "foo:Bar:fluff";

      Assert.False(HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAFormatException_WhenCorrectHashSaltElementIsEmpty()
    {
      string password = "Password2#";

      string correctHash = "123::fluff";

      Assert.Throws<FormatException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAFormatException_WhenCorrectHashPasswordHashElementIsEmpty()
    {
      string password = "Password2#";

      string correctHash = "123:Bar:";

      Assert.Throws<FormatException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAFormatException_WhenCorrectHashPasswordHashElementIsNotAMultipleOf4()
    {
      string password = "Password2#";

      string correctHash = "123:Bar:abc";

      Assert.Throws<FormatException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldThrowAFormatException_WhenCorrectHashSaltElementIsNotAMultipleOf4()
    {
      string password = "Password2#";

      string correctHash = "123:Bar:ABCD";

      Assert.Throws<FormatException>(() => HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldReturnTrue_WhenPasswordHashAndCorrectHashMatch()
    {
      string password = "Password2#";

      string correctHash = HashUtility.CreateHash(password);

      Assert.True(HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldReturnTrue_WhenPasswordHashAndCorrectHashMatchUsingCodePage1252Characters()
    {
      string password = ":€**‚ƒ„…†‡ˆ‰Š‹Œ**Ž****‘’“”•–—˜™š›œ**žŸ!\"¡#¢$£%¤&¥'¦(§)¨*©+ª,«-¬./®0¯1°2±3²4³5´6µ7¶8·9¸¹;º<»=¼>½?¾@¿AÀBÁCÂDÃEÄFÅGÆHÇIÈJÉKÊLËMÌNÍOÎPÏQÐRÑSÒTÓUÔVÕWÖX×YØZÙ[Ú\\Û]Ü^Ý_Þ`ßaàbácâdãeäfågæhçièjékêlëmìníoîpïqðrñsòtóuôvõwöx÷yøzù{ú|û}ü~ý";

      string correctHash = HashUtility.CreateHash(password);

      Assert.True(HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldReturnTrue_WhenPasswordHashAndCorrectHashMatchUsingAllPrintableAsciiCharacters()
    {
      string password = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

      string correctHash = HashUtility.CreateHash(password);

      Assert.True(HashUtility.AreEqual(password, correctHash));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.Security.Utilities.HashUtilityTests")]
    public void ValidatePassword_ShouldReturnFalse_WhenPasswordHashAndCorrectHashDoNotMatch()
    {
      string password = "Password2#";

      string correctHash = HashUtility.CreateHash("!something.Different*");

      Assert.False(HashUtility.AreEqual(password, correctHash));
    }

    private static bool PasswordHashIsValid(string passwordHash)
    {
      string[] split = passwordHash.Split(':');

      int interationCount;

      return split.Length == 3 && int.TryParse(split[0], out interationCount) && !string.IsNullOrWhiteSpace(split[1]) && !string.IsNullOrWhiteSpace(split[2]);
    }
  }
}
