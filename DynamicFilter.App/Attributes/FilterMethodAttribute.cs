using System;
using DynamicFilter.App.Enums;

namespace DynamicFilter.App.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FilterMethodAttribute: Attribute
    {
        public FilterMethodAttribute(FilterMethods methodName, string propertyName = null)
        {
            MethodName = methodName.ToString();
            PropertyName = propertyName;
        }
        public string MethodName { get; set; }
        public string PropertyName { get; set; }
    }
}
