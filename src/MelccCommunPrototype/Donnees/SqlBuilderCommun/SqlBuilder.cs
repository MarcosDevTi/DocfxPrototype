using Dapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Demo.Commun
{
    public class SqlBuilder : ISqlBuilder
    {
        private readonly IBuilderParams _builderParams;
        private readonly BuilderCount _builderCount;

        public SqlBuilder(IBuilderParams builderParams, BuilderCount builderCount)
        {
            _builderParams = builderParams;
            _builderCount = builderCount;
        }

        public SqlBuilder SelectFrom<T>(Requete request)
        {
            _builderParams.AjouterSelectFrom(request, typeof(T));
            return this;
        }

        public SqlBuilder SelectFrom<T>()
        {
            _builderParams.AjouterSelectFrom(typeof(T));
            return this;
        }

        public BuilderCount SelectCount<T>(Requete request)
        {
            StartBuiderCount<T>(request);
            return _builderCount;
        }

        public (string Sql, DynamicParameters Parameters) SelectFullCount<T>(Requete requete) =>
            SelectCount<T>(requete).WhereBuild();

        public (string Sql, DynamicParameters Parameters) GetSelectWithPaging<T>(Requete requete) =>
            SelectFrom<T>(requete).Where().Sort().Paging();

        public string GetInsert<T>() => typeof(T).Sql().ObtenirSqlInsert();

        public string GetUpdate<T>() => typeof(T).Sql().ObtenirUpdateSql();

        public string GetDeleteSql<T>() =>
            $"delete from {typeof(T).Sql().ObtenirNomTable()} where id = @Id";

        public string GetTruncate<T>() => $"delete from {GetTable<T>()}";

        public (string Sql, DynamicParameters Parameters) SqlObtenirUn<T>(Expression<Func<T, object>> expression, object value)
        {
            var name = expression.GetMemberName();

            var dbArgs = new DynamicParameters();
            dbArgs.Add(name, value);

            return (_builderParams.BuildObtenirUn(name), dbArgs);
        }

        public (string Sql, DynamicParameters Parameters) ExistsWithParam<T>(Expression<Func<T, object>> expression, object value)
        {
            var name = expression.GetMemberName();

            var dbArgs = new DynamicParameters();
            dbArgs.Add(name, value);

            return (typeof(T).Sql().ObtenirAuMoinsUnSql(name), dbArgs);
        }



        private string GetTable<T>() =>
            typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name.CaseSensitive();

        private void StartBuiderCount<T>(Requete requete) => _builderParams.AjouterSelectCount(requete, typeof(T));

        public override string ToString() => _builderParams.Build();

        public SqlBuilder Where()
        {
            _builderParams.AjouterWhere();
            return this;
        }

        public SqlBuilder Sort()
        {
            _builderParams.AjouterOrdenation();
            return this;
        }

        public (string Sql, DynamicParameters Parameters) Paging() =>
            _builderParams.ObtenirPagination();
    }

    public class BuilderCount
    {
        private readonly IBuilderParams _builderParams;

        public BuilderCount(IBuilderParams builderParams)
        {
            _builderParams = builderParams;
        }

        public (string Sql, DynamicParameters Parameters) WhereBuild()
        {
            _builderParams.AjouterWhere();

            return (_builderParams.Build(), _builderParams.ObtenirParametres());
        }
    }
}
