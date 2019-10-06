using System;
using System.Linq;
using System.Collections.Generic;
using DynamicFilter.Models;
using DynamicFilter.Attributes;

namespace DynamicFilter
{
    internal class FilterModelGenerator<T> where T : BaseFilter
    {
        private Type _forType;

        private List<FilterModel> _filters;
        internal List<FilterModel> Filters { get => _filters ?? (_filters = new List<FilterModel>()); }

        internal void GenerateFilterModel(T model)
        {
            var classAttributes = typeof(T).GetCustomAttributes(typeof(FilterForAttribute), false);
            var filterForAttribute = classAttributes.FirstOrDefault() as FilterForAttribute;
            if (filterForAttribute == null)
                throw new Exception("Filter not applied"); //TODO: Create custom FilterNotAppliedException

            _forType = filterForAttribute.ForType;

            model.Configure();
            var validationPredicates = model.Predicates;

            var props = model.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propertyAttributes = prop.GetCustomAttributes(typeof(FilterMethodAttribute), false);

                if (!propertyAttributes.Any())
                    continue;

                //Custom Validations
                if (validationPredicates.TryGetValue(prop.Name, out Func<object, bool> predicate))
                {
                    var shouldExecute = predicate.Invoke(model);
                    if (!shouldExecute)
                        continue;
                }

                foreach (FilterMethodAttribute methodAttribute in propertyAttributes)
                {
                    var filter = new FilterModel();

                    filter.MethodName = methodAttribute.MethodName;
                    filter.PropertyName = methodAttribute.PropertyName ?? prop.Name;
                    filter.Value = prop.GetValue(model);
                    filter.PropertyType = _forType.GetProperty(filter.PropertyName).PropertyType;
                    filter.ValueType = prop.PropertyType;
                    filter.ConditionalOperator = methodAttribute.ConditionalOperator;
                    filter.FilterPropertyName = prop.Name;

                    if (!filter.IsValid())
                        continue;

                    Filters.Add(filter);
                }
            }
        }
    }
}
