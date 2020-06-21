namespace Demo.Commun
{
    public static class StringExtensions
    {
        public static string CaseSensitive(this string txt, string attributeName = null)
        {
            return txt == null ?
                $"{attributeName.ConvertirPascalCaseUnderscore()} {attributeName}" :
                $"{txt.ConvertirPascalCaseUnderscore()} {attributeName}";
        }

        public static string CaseSensitiveSansAlias(this string txt, string attributeName = null)
        {
            return txt == null ?
                $"{attributeName.ConvertirPascalCaseUnderscore()}" :
                $"{txt.ConvertirPascalCaseUnderscore()}";
        }
    }
}
