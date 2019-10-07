using System.Globalization;
using Token.Core;
using Xunit;

namespace Token.Tests.Core
{
  public class StringResourceUtilityTests
  {
    [Theory]
    [InlineData("en-US", "This is a test.")]
    [InlineData("es-US","Esto es una prueba.")]
    public void Localizer_ShouldReturnCorrectValue_WhenCultureIsSet(string culture, string expectedValue)
    {
      CultureInfo.CurrentCulture = new CultureInfo(culture);
      CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

      string value = StringResourceUtility.Localizer["Test.Value"];

      Assert.Equal(expectedValue, value);
    }
  }
}
