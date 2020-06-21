using Dapper;
using System;
using System.Threading.Tasks;

namespace Demo.Commun
{
    public class RepoSet<T> : RepoSetLecture<T> where T : IPilotable
    {
        private readonly ISqlBuilder _sqlBuilder;

        public RepoSet(ISqlBuilder sqlBuilder, IConnectionFactory connectionFactory)
            : base(sqlBuilder, connectionFactory)
        {
            _sqlBuilder = sqlBuilder;
        }

        public async Task Supprimer(Guid id)
        {
            var sqlSupprimer = _sqlBuilder.GetDeleteSql<T>();
            await Connection.ExecuteAsync(sqlSupprimer, new { Id = id });

        }

        public async Task Creer(T[] objs)
        {
            var insertSql = _sqlBuilder.GetInsert<T>();
            var res = await Connection.ExecuteAsync(insertSql, objs);
        }


        public async Task<T> Creer(T obj)
        {
            var insertSql = _sqlBuilder.GetInsert<T>();
            var res = await Connection.ExecuteAsync(insertSql, obj);
            return obj;
        }

        public async Task<T> Modifier(T obj)
        {
            var updateSql = _sqlBuilder.GetUpdate<T>();
            var res = await Connection.ExecuteAsync(updateSql, obj);
            return obj;
        }

        public async Task Truncate()
        {
            var truncateSql = _sqlBuilder.GetTruncate<T>();
            var res = await Connection.ExecuteAsync(truncateSql);
        }
    }
}
