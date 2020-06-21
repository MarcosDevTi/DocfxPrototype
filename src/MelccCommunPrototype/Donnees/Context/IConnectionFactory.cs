using System;
using System.Data;

namespace Demo.Commun
{
    public interface IConnectionFactory
    {
        IDbConnection ObtenirConexion();
        string ObtenirNomTable(Type type);
    }
}
