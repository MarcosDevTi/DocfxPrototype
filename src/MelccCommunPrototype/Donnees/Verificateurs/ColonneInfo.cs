using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Commun
{
    [Table("information_schema.columns")]
    public class ColonneInfo
    {
        //public string TableName { get; set; }
        public string ColumnName { get; set; }
        //public string IsNullable { get; set; }
        //public string DataType { get; set; }
        //public int? CharacterMaximumLength { get; set; }
        //public string IsIdentity { get; set; }
    }
}
