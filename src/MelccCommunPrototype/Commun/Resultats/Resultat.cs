using System.Collections.Generic;
using System.Linq;

namespace Demo.Commun
{
    public class ResultatBase<T, E>
    {
        protected ResultatBase(T contenu, bool estUnSucces, E erreurs)
        {
            Contenu = contenu;
            EstUnSucces = estUnSucces;
            Erreurs = erreurs;
        }

        public T Contenu { get; }
        public bool EstUnSucces { get; }
        public virtual E Erreurs { get; }
    }

    public class Resultat<T> : ResultatBase<T, MessageErreur[]>
    {
        protected Resultat(T contenu, bool estUnSucces, MessageErreur[] erreurs)
            : base(contenu, estUnSucces, erreurs)
        {
        }
    }

    public class ResultatPagine<T>
    {
        public ResultatPagine(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
    }

    public class Succes<T> : Resultat<T>
    {
        public Succes(T contenu)
            : base(
                  contenu.VerifierSiNull(),
                  true,
                  Enumerable.Empty<MessageErreur>().ToArray())
        {
        }
    }

    public class Echec<T> : Resultat<T>
    {
        public Echec(MessageErreur[] erreurs)
            : base(default, false, erreurs)
        {
        }
    }
}