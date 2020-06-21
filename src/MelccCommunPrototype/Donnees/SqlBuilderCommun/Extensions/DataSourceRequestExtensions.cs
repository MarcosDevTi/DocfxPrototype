using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Demo.Commun
{
    public static class DataSourceRequestExtensions
    {
        public static (DynamicParameters ObjectParams, List<string> ListParams) GetWhere(this Requete request, Type type)
        {
            var dbArgs = new DynamicParameters();

            var listParam = new List<string>();
            dbArgs.Add("Row", request.GetRow());
            dbArgs.Add("PageSize", request.TaillePage);

            foreach (var filterDesc in request.Filtres)
            {
                dbArgs.Add(filterDesc.Membre, filterDesc.Valeur);
                listParam.Add(GetDescriptor(filterDesc, type));
            }

            return (dbArgs, listParam);
        }

        private static string GetDescriptor(Filtre filterDescriptor, Type type)
        {
            var filterDesc = (Filtre)filterDescriptor;
            var attrColumn = (type.GetProperty(filterDesc.Membre) ??
                              throw new InvalidOperationException("Operation is not valid due to the current state of the object."))
                .GetCustomAttribute<ColumnAttribute>()?.Name ?? filterDesc.Membre;
            return GetOperator(filterDesc.Operateur, attrColumn, filterDesc.Membre);
        }

        private static string GetOperator(OperateurFiltre filterOperator, string prop, string alias)
        {
            var propCase = prop.CaseSensitive();
            return filterOperator switch
            {
                OperateurFiltre.IsGreaterThan => $"{propCase} > @{alias}",
                OperateurFiltre.IsEqualTo => $"{propCase} = @{alias}",
                OperateurFiltre.IsLessThan => $"{propCase} < @{alias}",
                OperateurFiltre.StartsWith => $"{propCase} LIKE @{alias}||'%'",
                OperateurFiltre.EndsWith => $"{propCase} LIKE '%'||@{alias}",
                OperateurFiltre.Contains => $"{propCase} LIKE '%'||@{alias}||'%'",
                //OperateurFiltre.IsLessThanOrEqualTo => "",
                //OperateurFiltre.IsNotEqualTo => "",
                //OperateurFiltre.IsGreaterThanOrEqualTo => "",
                //OperateurFiltre.IsContainedIn => "",
                //OperateurFiltre.DoesNotContain => "",
                //OperateurFiltre.IsNull => "",
                //OperateurFiltre.IsNotNull => "",
                //OperateurFiltre.IsEmpty => "",
                //OperateurFiltre.IsNotEmpty => "",
                //OperateurFiltre.IsNullOrEmpty => "",
                //OperateurFiltre.IsNotNullOrEmpty => "",
                _ => ""
            };
        }
    }
}
