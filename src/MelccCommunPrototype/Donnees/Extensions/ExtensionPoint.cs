namespace Demo.Commun
{
    public class ExtensionPoint<T>
    {
        public ExtensionPoint(T extendValue)
        {
            ExtendValue = extendValue;
        }
        public T ExtendValue { get; set; }
    }
}
