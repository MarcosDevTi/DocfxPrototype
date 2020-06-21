using System;
using System.Data;

namespace Demo.Commun
{
    public abstract class ContextBase : IDisposable
    {
        protected readonly ISqlBuilder SqlBuilder;
        protected readonly IConnectionFactory ConnectionFactory;

        public IDbConnection Connection { get; private set; }
        protected ContextBase(ISqlBuilder sqlBuilder, IConnectionFactory connectionFactory)
        {
            SqlBuilder = sqlBuilder;
            Connection = connectionFactory.ObtenirConexion();
            ConnectionFactory = connectionFactory;
        }

        public RepoSet<T> Set<T>() where T : IPilotable => new RepoSet<T>(SqlBuilder, ConnectionFactory);
        public RepoSetLecture<T> SetLecture<T>() => new RepoSetLecture<T>(SqlBuilder, ConnectionFactory);
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
