using System;
using System.Globalization;
using System.Security.Cryptography;

namespace Token.Core.Security
{
  public static class HashUtility
  {
    private const int SaltByteSize = 24;
    private const int HashByteSize = 24;
    private const int IterationIndex = 0;
    private const int SaltIndex = 1;
    private const int HashIndex = 2;

    private const int HashSectionCount = 3;

    private static readonly string PepperValue = Configuration.File.GetSection("Application")["PepperValue"] ?? "";

    public static string CreateHash(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentNullException("value");
      }

      byte[] salt = GenerateSalt();

      int iterationCount = GenerateIterationCount();

      // Hash the value and encode the parameters
      byte[] hash = HashValue(value, salt, iterationCount, HashByteSize);

      return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", iterationCount, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    private static byte[] GenerateSalt()
    {
      return RandomNumberGenerator.GenerateRandomBytes(byteCountToGenerate: SaltByteSize);
    }

    private static int GenerateIterationCount()
    {
      Random iterationsRng = new Random();
      return iterationsRng.Next(1, 999);
    }

    public static bool AreEqual(string value, string hashedValue)
    {
      if (string.IsNullOrWhiteSpace(hashedValue))
      {
        throw new ArgumentNullException("hashedValue");
      }

      char[] delimiter = { ':' };

      string[] hashSections = hashedValue.Split(delimiter);

      if (hashSections.Length != HashSectionCount)
      {
        throw new InvalidOperationException("Hash Sections length does not equal the required Hash Section Count. Hash Sections length was found to be {0} and can only be {1}.");
      }

      int iterationCount = 0;

      if (!int.TryParse(hashSections[IterationIndex], out iterationCount))
      {
        return false;
      }

      byte[] salt = Convert.FromBase64String(hashSections[SaltIndex]);

      byte[] hash = Convert.FromBase64String(hashSections[HashIndex]);

      byte[] compareHash = HashValue(value, salt, iterationCount, hash.Length);

      return SlowEquals(hash, compareHash);
    }

    // This method prevents our passwords from being time-attacked. Do not optimize it.
    private static bool SlowEquals(byte[] a, byte[] b)
    {
      uint diff = (uint)a.Length ^ (uint)b.Length;

      for (int i = 0; i < a.Length && i < b.Length; i++)
      {
        diff |= (uint)(a[i] ^ b[i]);
      }

      return diff == 0;
    }

    private static byte[] HashValue(string value, byte[] salt, int iterationCount, int hashLength)
    {
      using (Rfc2898DeriveBytes numberGenerator = new Rfc2898DeriveBytes(value + PepperValue, salt))
      {
        numberGenerator.IterationCount = iterationCount;

        return numberGenerator.GetBytes(hashLength);
      }
    }
  }
}
