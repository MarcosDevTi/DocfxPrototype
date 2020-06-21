using System;

namespace Demo.Commun
{
    public class ContextSchema : ContextBase, IDisposable
    {
        public ContextSchema(ISqlBuilder sqlBuilder, IConnectionFactory connectionFactory)
            : base(sqlBuilder, connectionFactory) { }

        public RepoSetLecture<TableInfo> TableInfos => new RepoSetLecture<TableInfo>(SqlBuilder, ConnectionFactory);
    }
}
