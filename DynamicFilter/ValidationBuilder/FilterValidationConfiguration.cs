using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace DynamicFilter.ValidationBuilder
{
    public class FilterValidationConfiguration<TClass> where TClass : class
    {
        private readonly Dictionary<string, Func<object, bool>> _predicates;

        public FilterValidationConfiguration(Dictionary<string, Func<object, bool>> predicates)
        {
            _predicates = predicates;
        }

        public FilterPropertyConfiguration Property<T>(Expression<Func<TClass, T>> propertyExpression)
        {
            var body = propertyExpression.Body as MemberExpression;
            var property = body.Member as PropertyInfo;
            return new FilterPropertyConfiguration(property.Name, _predicates);
        }
    }
}
