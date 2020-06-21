using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Commun
{
    [Table("information_schema.tables")]
    public class TableInfo
    {
        public string TableName { get; set; }
        //public string TableCatalog { get; set; }
        //public string TableSchema { get; set; }
        //public string TableType { get; set; }
        //public string IsInsertableInto { get; set; }
    }
}
