using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Demo.Commun
{
    [ExcludeFromCodeCoverage]
    public class TablesVerificateur
    {
        private readonly ContextSchema _contextSchema;
        private readonly ISqlBuilderClauses _sqlBuilderClauses;
        private readonly IAssemblieHelper _assemblieHelper;

        public TablesVerificateur(ContextSchema contextSchema, ISqlBuilderClauses sqlBuilderClauses, IAssemblieHelper assemblieHelper, params Type[] references)
        {
            _contextSchema = contextSchema;
            _sqlBuilderClauses = sqlBuilderClauses;
            _assemblieHelper = assemblieHelper;
            ValiderRepoSets(references);
        }

        private IEnumerable<Type> ObtenirPilotables(Type[] references)
        {
            var listResult = new List<Type>();
            foreach (var reference in references)
            {
                var target = reference.Assembly;
                var assemblies = target.GetReferencedAssemblies()
                    .Select(Assembly.Load)
                    .ToList();
                assemblies.Add(target);
                var types = from t in assemblies.SelectMany(a => a.GetExportedTypes())
                            where t.GetInterfaces().Any(_ => _ == typeof(IPilotable))
                            select t;
                listResult.AddRange(types);
            }

            return listResult;
        }

        private bool ValiderRepoSets(params Type[] references)
        {
            var pilotables = ObtenirPilotables(references);

            foreach (var pilotable in pilotables)
            {
                VerifierIntegrite(pilotable);
            }

            return true;
        }

        private void VerifierIntegrite(Type type)
        {
            if (!_assemblieHelper.TableExistante(type))
                throw new Exception($"Table: {type.Sql().ObtenirNomTable()} inexistante");
            _assemblieHelper.ColoneeExistante(type);
        }






    }






}
