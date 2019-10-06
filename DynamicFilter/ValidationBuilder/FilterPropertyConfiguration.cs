using DynamicFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicFilter.ValidationBuilder
{
    public static class BaseFilterExtensions
    {
        public static FilterPropertyConfiguration<TFilterModel> FilterBy<TFilterModel, TProperty>(this TFilterModel filterModel, Expression<Func<TFilterModel, TProperty>> propertyExpr)
            where TFilterModel : BaseFilter
        {
            var body = propertyExpr.Body as MemberExpression;
            var property = body.Member as PropertyInfo;

            return new FilterPropertyConfiguration<TFilterModel>(property.Name, filterModel._predicates);
        }
    }

    public class FilterPropertyConfiguration<TFilterModel>
    {
        private readonly string _propertyName;
        private Dictionary<string, Func<object, bool>> _predicates;

        public FilterPropertyConfiguration(string propertyName, Dictionary<string, Func<object, bool>> predicates)
        {
            _propertyName = propertyName;
            _predicates = predicates;
        }

        public void If(Func<TFilterModel, bool> predicate)
        {
            _predicates.Add(_propertyName, o => predicate((TFilterModel)o));
        }
    }
}
