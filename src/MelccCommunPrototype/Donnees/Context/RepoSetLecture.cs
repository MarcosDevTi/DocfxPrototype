using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.Commun
{

    public class RepoSetLecture<T>
    {
        private readonly ISqlBuilder _sqlBuilder;
        protected readonly IDbConnection Connection;

        public RepoSetLecture(ISqlBuilder sqlBuilder, IConnectionFactory connectionFactory)
        {
            var test = typeof(T);
            _sqlBuilder = sqlBuilder;
            Connection = connectionFactory.ObtenirConexion();
        }


        public async Task<T> ObtenirUn(Expression<Func<T, object>> expression, object value)
        {
            var builder = _sqlBuilder.SelectFrom<T>().SqlObtenirUn(expression, value);
            return await Connection.QuerySingleOrDefaultAsync<T>(builder.Sql, builder.Parameters);
        }

        public async Task<TProjection> ObtenirUn<TProjection>(Expression<Func<TProjection, object>> expression, object value)
        {
            var builder = _sqlBuilder.SelectFrom<TProjection>().SqlObtenirUn(expression, value);
            return await Connection.QuerySingleOrDefaultAsync<TProjection>(builder.Sql, builder.Parameters);
        }

        public async Task<ResultatPagine<TProjection>> ObtenirPagine<TProjection>(Requete requete)
        {
            var (sqlSelect, parametersSelect) = _sqlBuilder.GetSelectWithPaging<TProjection>(requete);
            var (sqlCount, parametersCount) = _sqlBuilder.SelectFullCount<TProjection>(requete);

            return new ResultatPagine<TProjection>(
                data: await Connection.QueryAsync<TProjection>(sqlSelect, parametersSelect),
                total: await Connection.QuerySingleAsync<int>(sqlCount, parametersCount)
            );
        }

        public async Task<IEnumerable<TProjection>> ObtenirList<TProjection>(string sql = "")
        {
            var sqlSelect = _sqlBuilder.SelectFrom<TProjection>().ToString() + " " + sql;
            return await Connection.QueryAsync<TProjection>(sqlSelect);
        }

        public async Task<bool> Existe(Expression<Func<T, object>> expression, object value)
        {
            var sqlExiste = _sqlBuilder.ExistsWithParam(expression, value);
            return await Connection.QuerySingleOrDefaultAsync<bool>(sqlExiste.Sql, sqlExiste.Parameters);
        }


    }
}
