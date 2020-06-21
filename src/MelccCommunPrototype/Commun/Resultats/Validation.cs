using FluentValidation.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Demo.Commun.Fabriques;

namespace Demo.Commun
{
    public class Validation<T>
    {
        protected Validation(T contenu, ValidationResult resultatValidation)
        {
            Contenu = contenu;
            EstUnSucces = resultatValidation?.IsValid ?? true;
            ResultatValidation = resultatValidation;
        }

        public T Contenu { get; }
        public bool EstUnSucces { get; }
        public ValidationResult ResultatValidation { get; }

        public MessageErreur[] MessagesErreurs =>
            ResultatValidation?.Errors.Select(e => new MessageErreur(e.PropertyName, e.ErrorMessage)).ToArray()
            ?? Enumerable.Empty<MessageErreur>().ToArray();

        public async Task<Resultat<R>> SiSucces<R>(Func<T, Task<Resultat<R>>> enCasDeSucces)
            => EstUnSucces
                ? await enCasDeSucces(Contenu)
                : Echec<R>(MessagesErreurs);

        public Resultat<R> VersResultat<R>(R contenu)
            => EstUnSucces
                ? Succes(contenu)
                : Echec<R>(MessagesErreurs);

        public static implicit operator Resultat<T>(Validation<T> validation)
            => validation.EstUnSucces
                ? Succes(validation.Contenu)
                : Echec<T>(validation.MessagesErreurs);
    }

    public class MessageErreur
    {
        public MessageErreur(string nomPropriete, string message)
        {
            NomPropriete = nomPropriete;
            Message = message;
        }
        public string NomPropriete { get; set; }
        public string Message { get; set; }
    }

    public class Valide<T> : Validation<T>
    {
        public Valide(T contenu)
            : base(contenu, null)
        {
        }
    }

    public class Invalide<T> : Validation<T>
    {
        public Invalide(ValidationResult resultatValidation)
            : base(default, resultatValidation)
        {
        }
    }
}