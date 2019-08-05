using System;
using DynamicFilter.Enums;

namespace DynamicFilter.Attributes
{
    /// <summary>
    /// Attribute class for setting filter options for property
    /// </summary>
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

        /// <summary>
        /// Filter method name
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Property name on which filter should apply
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Conditional operator in case for multiple filter
        /// </summary>
        public ConditionalOperators? ConditionalOperator { get; set; }
    }
}
