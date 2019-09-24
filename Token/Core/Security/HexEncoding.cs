using System;

namespace Token.Core.Security
{
  public static class HexEncoding
  {
    public static string GetString(byte[] bytes)
    {
      if (bytes == null)
      {
        throw new ArgumentNullException("bytes");
      }

      char[] c = new char[bytes.Length * 2];

      byte b;
      int offset = 0;

      for (int idx = 0; idx < bytes.Length; idx++)
      {
        b = (byte)(bytes[idx] >> 4);

        c[offset++] = ConvertByteToChar(b);

        b = (byte)(bytes[idx] & 0x0F);

        c[offset++] = ConvertByteToChar(b);
      }

      return new string(c);
    }

    private static char ConvertByteToChar(byte b)
    {
      if (b > 9)
      {
        // convert the byte from 0x0A - 0x0F to 'a' - 'f' ... 87 is 'a' minus 10
        return (char)(b + 'a' - 10);
      }
      else
      {
        // convert the byte from 0 - 9 to to '0' - '9'
        return (char)(b + '0');
      }
    }

    public static byte[] GetBytes(string value)
    {
      if (value == null)
      {
        throw new ArgumentNullException("value");
      }

      if (value.Length % 2 != 0)
      {
        throw new ArgumentException("The length of the input string for HexEncoding.GetBytes must be divisible by two.");
      }

      if (value.Length == 0)
      {
        return new byte[0];
      }

      int offset = 0;
      byte[] buffer = new byte[value.Length / 2];

      for (int idx = 0; idx < buffer.Length; idx++)
      {
        // convert first half of byte
        buffer[idx] = (byte)(ConvertCharToByte(value[offset++]) << 4);

        // convert second half of byte
        buffer[idx] |= ConvertCharToByte(value[offset++]);
      }

      return buffer;
    }

    public static byte ConvertCharToByte(char c)
    {
      if (c > '9')
      {
        // check to see if c is uppercase 'A' - 'F'
        if (c >= 'A' && c <= 'F')
        {
          // convert the char from 'A' - 'F' to 0x0A - 0x0F  ... 55 is 'A' minus 10
          return (byte)(c - 55);
        }
        else if (c >= 'a' && c <= 'f')  // c is lowercase 'a' - 'f'
        {
          // convert the char from 'a' - 'f' to 0x0A - 0x0F ... 87 is 'a' minus 10
          return (byte)(c - 87);
        }
      }
      else if (c >= '0')
      {
        // convert the char from '0' - '9' to to 0 - 9
        return (byte)(c - '0');
      }

      throw new ArgumentOutOfRangeException("c", c, "The value passed in must be between 0 and 9 or a - f");
    }
  }
}
