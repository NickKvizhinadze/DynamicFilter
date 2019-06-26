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
        internal List<FilterModel> Filters { get; private set; }

        internal void GenerateFilterModel(T model)
        {
            var classAttributes = typeof(T).GetCustomAttributes(typeof(FilterForAttribute), false);
            var filterForAttribute = classAttributes.FirstOrDefault() as FilterForAttribute;
            if (filterForAttribute == null)
                throw new Exception("Filter not applied"); //TODO: Create custom FilterNotAppliedException

            _forType = filterForAttribute.ForType;

            model.Configure();
            var validationPredicates = model.GetPredicates();

            var props = model.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propertyAttributes = prop.GetCustomAttributes(typeof(FilterMethodAttribute), false);
                if (propertyAttributes?.Any() != true)
                    continue;

                var methodAttribute = propertyAttributes.First() as FilterMethodAttribute;
                var filter = new FilterModel();

                filter.MethodName = methodAttribute.MethodName;
                filter.PropertyName = methodAttribute.PropertyName ?? prop.Name;
                filter.Value = prop.GetValue(model);
                filter.PropertyType = _forType.GetProperty(filter.PropertyName).PropertyType;
                filter.ValueType = prop.PropertyType;

                if (!filter.IsValid())
                    continue;

                //Custom Validations
                if (validationPredicates.TryGetValue(filter.PropertyName, out Func<object, bool> predicate))
                {
                    var shouldExecute = predicate.Invoke(filter.Value);
                    if (!shouldExecute)
                        continue;
                }
                                
                if (Filters == null)
                    Filters = new List<FilterModel>();
                Filters.Add(filter);
            }

        }
    }
}
