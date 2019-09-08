using System;
using Token.Core.Security;
using Xunit;

namespace Tests
{
  public class HexEncodingTests
  {
    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldThrowArgumentNullException_WhenNullIsPassedIn()
    {
      Assert.Throws<ArgumentNullException>(() => HexEncoding.GetString(null));
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturnEmptyString_WhenAnEmptyArrayIsPassedIn()
    {
      string value = HexEncoding.GetString(new byte[0]);

      Assert.Equal("", value);
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturn00_When0IsPassedIn()
    {
      string value = HexEncoding.GetString(new byte[] { 0 });

      Assert.Equal("00", value);
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturnLowByteValues_WhenOnlyLowByteValuesArePassedIn()
    {
      string value = HexEncoding.GetString(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

      Assert.Equal("0102030405060708090a0b0c0d0e0f", value);
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturnHighByteValues_WhenOnlyHighByteValuesArePassedIn()
    {
      string value = HexEncoding.GetString(new byte[] { 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240 });

      Assert.Equal("102030405060708090a0b0c0d0e0f0", value);
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturnAllFs_WhenAllBitsAreSet()
    {
      string value = HexEncoding.GetString(new byte[] { 255, 255, 255, 255 });

      Assert.Equal("ffffffff", value);
    }

    [Fact]
    [Trait("Category", "ARX.UnitTests.Libraries.ARX.Core.HexEncodingTests")]
    public void ShouldReturnAllLowercaseLetters_WhenAskingForAllLowercase()
    {
      string value = HexEncoding.GetString(new byte[] { 170, 170, 170, 170 });

      Assert.Equal("aaaaaaaa", value);
    }
  }
}
