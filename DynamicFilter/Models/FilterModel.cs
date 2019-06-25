using DynamicFilter.Extentions;
using System;

namespace DynamicFilter.Models
{
    public class FilterModel
    {
        #region Constructor
        public FilterModel()
        {
        }

        public FilterModel(string propertyname, Type valueType, object value, string methodName)
        {
            PropertyName = propertyname;
            PropertyType = valueType;
            Value = value;
            MethodName = methodName;
        }
        #endregion

        #region Properties
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }
        public string MethodName { get; set; }
        #endregion

        #region Methods
        public bool IsValid()
        {
            //TODO: Save invaled fields and error messages
            if (string.IsNullOrEmpty(PropertyName))
                return false;

            if (PropertyType == null)
                return false;
            
            if (Value == null || Value.IsNotNullOrEmptyArray())
                return false;

            if (string.IsNullOrEmpty(MethodName))
                return false;
            return true;
        }
        #endregion
    }
}

