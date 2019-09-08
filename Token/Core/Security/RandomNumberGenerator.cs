using System;
using System.Security.Cryptography;

namespace Token.Core.Security
{
  public static class RandomNumberGenerator
  {
    public static string GenerateRandomHexString(int charCountToGenerate)
    {
      if (charCountToGenerate > int.MaxValue - 1 || charCountToGenerate <= 0)
      {
        throw new ArgumentOutOfRangeException("charCountToGenerate");
      }

      byte[] randomBytes = GenerateRandomBytes((charCountToGenerate + 1) / 2);

      // Convert the bytes to a hex encoded string (length = 2 * (length of byte array)).
      return HexEncoding.GetString(randomBytes).Substring(0, charCountToGenerate);
    }

    public static byte[] GenerateRandomBytes(int byteCountToGenerate)
    {
      if (byteCountToGenerate <= 0)
      {
        throw new ArgumentOutOfRangeException("byteCountToGenerate");
      }

      byte[] randomBytes = new byte[byteCountToGenerate];

      using (RNGCryptoServiceProvider randomGenerator = new RNGCryptoServiceProvider())
      {
        randomGenerator.GetBytes(randomBytes);
      }

      return randomBytes;
    }

    public static void GenerateRandomBytes(int byteCountToGenerate, out byte[] randomBytes1, out byte[] randomBytes2)
    {
      if (byteCountToGenerate <= 0)
      {
        throw new ArgumentOutOfRangeException("byteCountToGenerate");
      }

      randomBytes1 = new byte[byteCountToGenerate];
      randomBytes2 = new byte[byteCountToGenerate];

      using (RNGCryptoServiceProvider randomGenerator = new RNGCryptoServiceProvider())
      {
        randomGenerator.GetBytes(randomBytes1);
        randomGenerator.GetBytes(randomBytes2);
      }

      return;
    }
  }
}
