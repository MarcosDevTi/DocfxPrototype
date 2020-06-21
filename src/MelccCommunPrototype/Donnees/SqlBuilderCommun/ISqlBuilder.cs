using Dapper;
using System;
using System.Linq.Expressions;

namespace Demo.Commun
{
    public interface ISqlBuilder
    {
        SqlBuilder SelectFrom<T>(Requete request);
        SqlBuilder SelectFrom<T>();
        BuilderCount SelectCount<T>(Requete request);
        (string Sql, DynamicParameters Parameters) SelectFullCount<T>(Requete requete);
        (string Sql, DynamicParameters Parameters) GetSelectWithPaging<T>(Requete requete);
        string GetInsert<T>();
        string GetUpdate<T>();
        string GetDeleteSql<T>();
        string GetTruncate<T>();
        (string Sql, DynamicParameters Parameters) SqlObtenirUn<T>(Expression<Func<T, object>> expression, object value);
        (string Sql, DynamicParameters Parameters) ExistsWithParam<T>(Expression<Func<T, object>> expression, object value);
    }
}
