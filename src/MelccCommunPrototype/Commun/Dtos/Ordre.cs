using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Demo.Commun
{
    public class Ordre
    {
        public Ordre(ListSortDirection directionOrdre, string membre)
        {
            DirectionOrdre = directionOrdre;
            Membre = membre;
        }
        public ListSortDirection DirectionOrdre { get; set; }
        public string Membre { get; set; }

        public string GetAttributeColumn<T>() =>
            GetAttributeColumn(typeof(T));

        public string GetAttributeColumn(Type type)
        {
            var propertyResult = type?.GetProperty(Membre ?? throw new InvalidOperationException());
            if (propertyResult == null)
            {
                throw new ArgumentException(
                    $"The {Membre} attribute does not correspond to any object properties.");
            }
            return propertyResult?.GetCustomAttribute<ColumnAttribute>()?.Name ?? Membre;
        }
    }
}
