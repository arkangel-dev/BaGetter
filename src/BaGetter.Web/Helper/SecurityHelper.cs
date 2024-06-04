using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Web.Helper;
internal static class SecurityHelper
{
    public const string TokenChars = "1234567890ABCDEFGHIJKLMNOPabcdefghijklmnop";
    public static Random? Rng = null;

    public static void InitRandom()
    {
        if (Rng is null)
        {
            Rng = new Random();
        }
    }
    public static string GenerateToken(int len = 32)
    {
        InitRandom();
        var token = new StringBuilder();
        for (int i = 0; i < len; i++)
        {
            token.Append(TokenChars[Rng.Next(0, TokenChars.Length)]);
        }
        return token.ToString();
    }
}
