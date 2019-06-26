using DynamicFilter.Attributes;
using DynamicFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicFilter
{
    public class FilterModelGenerator<T>
    {
        private Type _forType;
        public List<FilterModel> Filters { get; private set; }

        public void GenerateFilterModel(T model)
        {
            var classAttributes = typeof(T).GetCustomAttributes(typeof(FilterForAttribute), false);
            var filterForAttribute = classAttributes.FirstOrDefault() as FilterForAttribute;
            if (filterForAttribute == null)
                throw new Exception("Filter not applied"); //TODO: Create custom FilterNotAppliedException

            _forType = filterForAttribute.ForType;

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
                if (Filters == null)
                    Filters = new List<FilterModel>();
                Filters.Add(filter);
            }

        }
    }
}
