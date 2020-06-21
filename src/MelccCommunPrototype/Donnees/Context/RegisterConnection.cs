namespace Demo.Commun
{
    public class RegisterConnection
    {
        public string ConnectionStrings { get; private set; }
        public virtual string Prefix { get; private set; }
        public RegisterConnection(string connectionStrings, string prefix = null)
        {
            ConnectionStrings = connectionStrings;
            Prefix = prefix;
        }
    }
}
