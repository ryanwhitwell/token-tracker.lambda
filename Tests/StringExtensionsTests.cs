using Xunit;
using Token.Core.StringExtensions;

namespace Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("abc123", "ab****")]
        [InlineData("abc123d", "ab*****")]
        [InlineData("abc123de", "ab******")]
        [InlineData("abc123def", "abc******")]
        [InlineData("95#exDwB$x87c2ZAB^$L4", "95#exD***************")]
        public void Mask(string source, string expectedResult)
        {
           string result = source.Mask();
           Assert.Equal(expectedResult, result);
        }
    }
}
