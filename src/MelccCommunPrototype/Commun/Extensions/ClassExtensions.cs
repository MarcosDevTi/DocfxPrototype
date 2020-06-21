using System;

namespace Demo.Commun
{
    public static class ClassExtensions
    {
        public static T VerifierSiNull<T>(this T valeur)
            => valeur == null
                ? throw new ArgumentNullException(nameof(valeur))
                : valeur;
    }
}