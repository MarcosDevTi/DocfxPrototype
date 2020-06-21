namespace Demo.Commun
{
    public static class ExtensionPoints
    {
        public static SqlBuilderExtensionPoint<T> Sql<T>(this T value) =>
            new SqlBuilderExtensionPoint<T>(value);
    }
}
