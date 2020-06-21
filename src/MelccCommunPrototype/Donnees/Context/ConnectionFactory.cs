using Npgsql;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Demo.Commun
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IDbConnection _dbConnection;
        private readonly RegisterConnection _registerConnection;
        public ConnectionFactory(RegisterConnection registerConnection)
        {
            _dbConnection = ConstructionConextion(registerConnection.ConnectionStrings);
            _registerConnection = registerConnection;
        }

        private IDbConnection ConstructionConextion(string connectionStrings) =>
            new ProfiledDbConnection(new NpgsqlConnection(connectionStrings), MiniProfiler.Current);

        public IDbConnection ObtenirConexion() => _dbConnection;

        public string ObtenirNomTable(Type type)
        {
            var prefix = "";
            var estPilotable = typeof(IPilotable).IsAssignableFrom(type);

            prefix += estPilotable ? _registerConnection.Prefix + "_" :
                _registerConnection.Prefix + "_vw_";

            var nomTable = type.GetCustomAttribute<TableAttribute>()?.Name
                           ?? prefix + type.Name.CaseSensitive();
            return nomTable.Replace(" ", "");
        }
    }
}
