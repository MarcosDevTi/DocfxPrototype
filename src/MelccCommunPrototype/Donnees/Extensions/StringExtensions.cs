using System.Linq;

namespace Demo.Commun
{
    public static class StringExtensionsA
    {
        /// <summary>
        /// Convertir de Pascal Case ver le format de la base de donnée
        /// </summary>
        /// <param name="txt">texte</param>
        /// <returns>texte converti</returns>
        public static string ConvertirPascalCaseUnderscore(this string txt)
        {
            return txt != null ? string.Concat(txt.Select((x, i) => (i > 0 && char.IsUpper(x))
            || int.TryParse(x.ToString(), out _) ? "_" + x.ToString() : x.ToString())).ToLower() : null;
        }


    }
}
