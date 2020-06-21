using System.Collections.Generic;

namespace Demo.Commun
{
    public class Requete
    {
        public int Page { get; set; }
        public int TaillePage { get; set; }
        public IReadOnlyList<Ordre> Ordres { get; set; }
        public IReadOnlyList<Filtre> Filtres { get; set; }

        public int GetRow() => TaillePage * (Page - 1);
    }
}
