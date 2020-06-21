using System.Linq;

namespace Env.Commun.Core
{
    public static class StringUtils
    {
        public static string SeulementNumero(this string str, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}