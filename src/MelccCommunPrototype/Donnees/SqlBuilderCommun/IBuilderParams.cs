using Dapper;
using System;

namespace Demo.Commun
{
    public interface IBuilderParams
    {
        void AjouterSelectFrom(Requete request, Type type);
        void AjouterSelectFrom(Type type);
        void AjouterSelectCount(Requete requete, Type type);
        (string Sql, DynamicParameters Parameters) ObtenirPagination();
        void AjouterWhere();
        void AjouterOrdenation();
        string BuildObtenirUn(string nom);
        string Build();
        DynamicParameters ObtenirParametres();
    }
}
