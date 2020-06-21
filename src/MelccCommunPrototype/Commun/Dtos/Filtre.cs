namespace Demo.Commun
{
    public class Filtre
    {
        public Filtre(string membre, OperateurFiltre operateur, object valeur)
        {
            Membre = membre;
            Operateur = operateur;
            Valeur = valeur;
        }
        public string Membre { get; }
        public OperateurFiltre Operateur { get; }
        public object Valeur { get; set; }
    }
}
