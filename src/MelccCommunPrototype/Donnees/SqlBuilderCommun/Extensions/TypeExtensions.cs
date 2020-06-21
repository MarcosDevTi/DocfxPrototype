using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Demo.Commun
{
    public static class TypeExtensionsA
    {
        public static string[] ObtenirProps(this SqlBuilderExtensionPoint<Type> type)
        {
            return type.ExtendValue.GetProperties()
                    .Where(_ => _.GetCustomAttribute<IgnoreSqlAttribute>() == null)
                    .Select(GetAttributeColumn).ToArray();


        }

        public static string ObtenirSqlInsert(this SqlBuilderExtensionPoint<Type> type)
        {
            var (properties, values) = type.ObtenirProprietesEtValeurs();
            var sql = new StringBuilder()
                .AppendLine($"insert into {type.ExtendValue.Sql().ObtenirTable()}")
                .AppendLine($"({ string.Join(", ", properties.Select(_ => $"{_.CaseSensitive()}"))})")
                .AppendLine("values")
                .AppendLine($"({string.Join(", ", values)})").ToString();
            return sql;
        }

        public static string ObtenirTable(this SqlBuilderExtensionPoint<Type> type) =>
            type.ExtendValue.GetCustomAttribute<TableAttribute>()?.Name ?? type.ExtendValue.Name.CaseSensitive();

        public static (List<string> Properties, List<string> Values) ObtenirProprietesEtValeurs(
            this SqlBuilderExtensionPoint<Type> type)
        {
            var propsList = new List<string>();
            var valuesList = new List<string>();
            type.ExtendValue.GetProperties().ToList().ForEach(_ =>
            {
                if (_.PropertyType.IsConstructedGenericType ||
                    _.PropertyType == typeof(ValidationResult) ||
                    _.GetCustomAttribute<IgnoreSqlAttribute>() != null) return;
                propsList.Add((_.GetCustomAttribute<ColumnAttribute>()?.Name ?? _.Name).CaseSensitive());
                valuesList.Add($"@{_.Name}");
            });

            return (propsList, valuesList);
        }

        public static string ObtenirAuMoinsUnSql(this SqlBuilderExtensionPoint<Type> type, string nomParam)
        {
            var sb = new StringBuilder();
            sb.AppendLine("select case");
            sb.AppendLine("    when exists (");
            sb.AppendLine("        select 1");
            sb.AppendLine($"        from {type.ObtenirNomTable()}");
            sb.AppendLine($"        where {nomParam.CaseSensitive()} = @{nomParam})");
            sb.AppendLine("    then 1 else 0");
            sb.AppendLine("end");

            return sb.ToString();
        }

        public static IEnumerable<string> ObtenirProprietesEtValuesUpdate(
            this SqlBuilderExtensionPoint<Type> type)
        {
            var result = new List<(string Property, string Value)>();

            type.ExtendValue.GetProperties().ToList().ForEach(_ =>
            {
                if (!_.PropertyType.IsConstructedGenericType
                    && _.PropertyType != typeof(ValidationResult)
                    && _.GetCustomAttribute<IgnoreSqlAttribute>() == null)
                {
                    result.Add((_.ObtenirNomColonne(), $"@{_.Name}"));
                }
            });
            return result.Select(_ => $"{_.Property.CaseSensitive()} = {_.Value}");
        }

        public static string ObtenirUpdateSql(this SqlBuilderExtensionPoint<Type> type)
        {
            var sql = new StringBuilder()
                .AppendLine($"update {type.ObtenirTable()} set")
                .AppendLine(
                    $"{ string.Join(", ", type.ObtenirProprietesEtValuesUpdate())}")
                .AppendLine("where id = @Id").ToString();
            return sql;
        }

        public static string GetAttributeColumn(PropertyInfo propertyInfo)
        {
            var attr = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name;
            return attr.CaseSensitive(propertyInfo.Name);
        }

        public static string GetParamsWithCommas(this SqlBuilderExtensionPoint<Type> type) =>
            string.Join(", ", type.ExtendValue.Sql().ObtenirProps());

        public static string GetMemberName<T>(this Expression<T> expression)
        {
            var result = expression.Body switch
            {
                MemberExpression m => (m.Member.GetCustomAttribute<ColumnAttribute>()?.Name ??
                                      m.Member.Name),
                UnaryExpression u when u.Operand is MemberExpression m =>
                m.Member.GetCustomAttribute<ColumnAttribute>()?.Name ??
                m.Member.Name,
                _ => throw new NotImplementedException(expression.GetType().ToString())
            };

            return result;
        }

        public static string ObtenirNomTable(this SqlBuilderExtensionPoint<Type> type) =>
             type.ExtendValue.GetCustomAttribute<TableAttribute>()?.Name ?? type.ExtendValue.Name.CaseSensitive();

        public static string ObtenirNomColonne(this PropertyInfo type) =>
            type.GetCustomAttribute<ColumnAttribute>()?.Name ?? type.Name.CaseSensitive();
    }
}
