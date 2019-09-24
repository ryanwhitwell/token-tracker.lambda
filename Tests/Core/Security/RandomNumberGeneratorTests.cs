using System;
using Token.Core.Security;
using Xunit;

namespace Token.Tests.Core.Security
{
  public class RandomNumberGeneratorTests
  {
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void GenerateRandomHexString_ParameterizedTests(int charCountToGenerate)
    {
      // Arrange

      // Act
      string result = RandomNumberGenerator.GenerateRandomHexString(charCountToGenerate);

      // Assert
      Assert.Equal(charCountToGenerate, result.Length);
      Assert.Equal(charCountToGenerate, result.Trim().Length);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    public void GenerateRandomHexString_ShouldThrowException_ParameterizedTests(int charCountToGenerate)
    {
      // Arrange

      // Act & Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => RandomNumberGenerator.GenerateRandomHexString(charCountToGenerate));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void GenerateRandomBytesForOneValue_ParameterizedTests(int byteCountToGenerate)
    {
      // Arrange

      // Act
      byte[] result = RandomNumberGenerator.GenerateRandomBytes(byteCountToGenerate);

      // Assert
      Assert.Equal(byteCountToGenerate, result.Length);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void GenerateRandomBytesForOneValue_ShouldThrowException_ParameterizedTests(int byteCountToGenerate)
    {
      // Arrange

      // Act & Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => RandomNumberGenerator.GenerateRandomBytes(byteCountToGenerate));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void GenerateRandomBytesForTwoValues_ParameterizedTests(int byteCountToGenerate)
    {
      // Arrange
      byte[] result1;
      byte[] result2;

      // Act
      RandomNumberGenerator.GenerateRandomBytes(byteCountToGenerate, out result1, out result2);

      // Assert
      Assert.Equal(byteCountToGenerate, result1.Length);
      Assert.Equal(byteCountToGenerate, result2.Length);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void GenerateRandomBytesForTwoValues_ShouldThrowException_ParameterizedTests(int byteCountToGenerate)
    {
      // Arrange
      byte[] result1;
      byte[] result2;

      // Act & Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => RandomNumberGenerator.GenerateRandomBytes(byteCountToGenerate, out result1, out result2));
    }
  }
}
