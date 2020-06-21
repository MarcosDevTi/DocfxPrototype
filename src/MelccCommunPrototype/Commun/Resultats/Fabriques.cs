using FluentValidation.Results;
using System.Threading.Tasks;

namespace Demo.Commun
{
    public static class Fabriques
    {
        public static Validation<T> Valide<T>(T contenu)
            => new Valide<T>(contenu);

        public static Validation<T> Invalide<T>(ValidationResult erreurs)
            => new Invalide<T>(erreurs);

        public static Resultat<T> Succes<T>(T contenu)
            => new Succes<T>(contenu);

        public static Resultat<T> Echec<T>(params MessageErreur[] erreurs)
            => new Echec<T>(erreurs);

        public static Task<T> Async<T>(this T valeur)
            => Task.FromResult(valeur);
    }
}