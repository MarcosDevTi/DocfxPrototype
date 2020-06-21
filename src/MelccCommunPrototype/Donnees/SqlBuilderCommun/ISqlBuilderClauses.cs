namespace Demo.Commun
{
    public interface ISqlBuilderClauses
    {
        SqlBuilderClauses.Template AddTemplateWhere<T>(dynamic parameters = null);
        SqlBuilderClauses OrWhere(string sql, dynamic parameters = null);
    }
}
