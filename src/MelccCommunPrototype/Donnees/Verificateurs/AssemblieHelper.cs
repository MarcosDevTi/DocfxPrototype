using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Demo.Commun
{
    [ExcludeFromCodeCoverage]
    public class AssemblieHelper : IAssemblieHelper
    {
        private readonly ContextSchema _contextSchema;
        private readonly ISqlBuilderClauses _sqlBuilderClauses;

        public AssemblieHelper(ContextSchema contextSchema, ISqlBuilderClauses sqlBuilderClauses)
        {
            _contextSchema = contextSchema;
            _sqlBuilderClauses = sqlBuilderClauses;
        }

        public bool TableExistante(Type type)
        {
            var nomTable = type.Sql().ObtenirNomTable();
            return _contextSchema.TableInfos.Existe(_ => _.TableName, nomTable).Result;
        }

        public bool ColoneeExistante(Type objType)
        {
            var nomTable = objType.Sql().ObtenirNomTable();

            _sqlBuilderClauses.OrWhere("table_name= @nomTable", new { nomTable });
            var t = _sqlBuilderClauses.AddTemplateWhere<ColonneInfo>();

            var res = _contextSchema.Connection.Query<ColonneInfo>(t.RawSql, t.Parameters);

            var listProps = objType.GetProperties()
                .Where(_ => _.GetCustomAttribute<IgnoreSqlAttribute>() == null)
                .Select(_ => (NomColonne: GetAttributeColumn(_), NomProp: _.Name));

            var nonExists = listProps
                .Where(prop => res.All(_ => _.ColumnName != prop.NomColonne)).ToList();

            if (nonExists.Any())
            {
                throw new Exception(FormatListErrors(objType, nonExists));
            }

            return _contextSchema.TableInfos.Existe(_ => _.TableName, nomTable).Result;
        }

        private static string GetAttributeColumn(PropertyInfo propertyInfo)
        {
            var attr = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name;
            return attr.CaseSensitiveSansAlias(propertyInfo.Name);
        }

        private string FormatListErrors(Type type, List<(string NomColonne, string NomProp)> errors)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Le mappings pour '{type.Sql().ObtenirNomTable()}' est manquante pour les colonnes: ");
            errors.ForEach(_ => sb.AppendLine($"['{_.NomProp}' vers => '{_.NomColonne}' ]"));
            return sb.ToString();
        }
    }

    public interface IAssemblieHelper
    {
        bool TableExistante(Type type);
        bool ColoneeExistante(Type objType);
    }


}
