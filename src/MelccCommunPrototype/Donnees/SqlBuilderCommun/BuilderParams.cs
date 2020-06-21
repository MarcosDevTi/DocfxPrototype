using Dapper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Demo.Commun
{
    public class BuilderParams : IBuilderParams
    {
        private Type _type;
        private StringBuilder _sb;
        private DynamicParameters _objectParameters;
        private string[] _paramsSql;
        private Ordre _sortDescriptor;

        private readonly IConnectionFactory _connectionFactory;


        public BuilderParams(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void AjouterSelectFrom(Requete request, Type type)
        {
            var tt = _connectionFactory.ObtenirNomTable(type);
            var (dynamicParameters, listParams) = request.GetWhere(type);
            _type = type;
            _sb = new StringBuilder();
            _objectParameters = dynamicParameters;
            _paramsSql = listParams.ToArray();
            _sortDescriptor = request.Ordres?.FirstOrDefault();
            _sb.AppendLine($"select {_type.Sql().GetParamsWithCommas()}");
            _sb.AppendLine($"from {_connectionFactory.ObtenirNomTable(type)}");
        }

        public void AjouterSelectFrom(Type type)
        {
            _type = type; ;
            _sb = new StringBuilder();
            _sb.AppendLine($"select {_type.Sql().GetParamsWithCommas()}");
            _sb.AppendLine($"from {_connectionFactory.ObtenirNomTable(_type)}");
        }

        public void AjouterSelectCount(Requete requete, Type type)
        {
            var rr = _connectionFactory.ObtenirNomTable(type);
            _type = type;
            _sb = new StringBuilder();
            var (parameters, listParamsWhere) = requete.GetWhere(type);
            _paramsSql = listParamsWhere.ToArray();
            _sb.AppendLine($"select count(*) from {_connectionFactory.ObtenirNomTable(_type)}");
        }

        public (string Sql, DynamicParameters Parameters) ObtenirPagination()
        {
            _sb.AppendLine($"offset @Row rows fetch next @PageSize rows only");
            return (_sb.ToString(), _objectParameters);
        }

        public void AjouterWhere()
        {
            if (_paramsSql.Any())
                _sb.AppendLine("where " + string.Join(" and ", _paramsSql));
        }

        public void AjouterOrdenation()
        {
            if (_sortDescriptor != null)
            {
                var order = _sortDescriptor?.DirectionOrdre == ListSortDirection.Ascending ? "asc" : "desc";
                _sb.AppendLine(
                    $"order by {_sortDescriptor.GetAttributeColumn(_type).CaseSensitive()} {order}");
            }
            else
                _sb.AppendLine($"order by id");
        }

        public string BuildObtenirUn(string nom)
        {
            _sb.AppendLine($"where {nom.CaseSensitive()} = @{nom}");
            return _sb.ToString();
        }

        public string Build() => _sb.ToString();

        public DynamicParameters ObtenirParametres() => _objectParameters;
    }
}
