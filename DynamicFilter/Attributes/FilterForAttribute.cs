using System;

namespace DynamicFilter.Attributes
{
    /// <summary>
    /// Attribute Class for setting filtering data type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FilterForAttribute: Attribute
    {
        public FilterForAttribute(Type type)
        {
            ForType = type;
        }

        /// <summary>
        /// Filtering data type
        /// </summary>
        public Type ForType { get; set; }
    }
}
