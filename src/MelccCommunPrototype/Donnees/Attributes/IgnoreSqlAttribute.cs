using System;

namespace Demo.Commun
{
    /// <summary>
    ///     Names the backing field associated with this property or navigation property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreSqlAttribute : Attribute
    {
    }
}
