using System;
using DynamicFilter.Enums;

namespace DynamicFilter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class FilterMethodAttribute : Attribute
    {
        public FilterMethodAttribute(FilterMethods methodName, string propertyName = null)
        {
            MethodName = methodName.ToString();
            PropertyName = propertyName;
        }

        public FilterMethodAttribute(FilterMethods methodName, ConditionalOperators conditionalOperator, string propertyName = null)
            : this(methodName, propertyName)
        {
            ConditionalOperator = conditionalOperator;
        }

        public string MethodName { get; set; }
        public string PropertyName { get; set; }
        public ConditionalOperators? ConditionalOperator { get; set; }
    }
}
