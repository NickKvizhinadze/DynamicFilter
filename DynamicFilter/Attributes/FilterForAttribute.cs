using System;

namespace DynamicFilter.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FilterForAttribute: Attribute
    {
        public FilterForAttribute(Type type)
        {
            ForType = type;
        }
        public Type ForType { get; set; }
    }
}
