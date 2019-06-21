using System;

namespace DynamicFilter.App.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FilterMethodAttribute: Attribute
    {
        public FilterMethodAttribute(string methodName, string propertyName = null)
        {
            MethodName = methodName;
            PropertyName = propertyName;
        }
        public string MethodName { get; set; }
        public string PropertyName { get; set; }
    }
}
