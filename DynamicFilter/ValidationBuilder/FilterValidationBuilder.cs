using System;
using System.Collections.Generic;

namespace DynamicFilter.ValidationBuilder
{
    public class FilterValidationBuilder
    {
        private Dictionary<string, Func<object, bool>> _predicates;

        public FilterValidationBuilder(Dictionary<string, Func<object, bool>> predicates)
        {
            _predicates = predicates;
        }
        public FilterValidationConfiguration<T> For<T>() where T : class
        {
            return new FilterValidationConfiguration<T>(_predicates);
        }
    }
}
