using System;
using System.Collections.Generic;

namespace DynamicFilter.ValidationBuilder
{
    public class FilterPropertyConfiguration
    {
        private readonly string _propertyName;
        private Dictionary<string, Func<object, bool>> _predicates;

        public FilterPropertyConfiguration(string propertyName, Dictionary<string, Func<object, bool>> predicates)
        {
            _propertyName = propertyName;
            _predicates = predicates;
        }

        public void AddValidation(Func<object, bool> predicate)
        {
            _predicates.Add(_propertyName, predicate);
        }
    }
}
