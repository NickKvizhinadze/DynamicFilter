using System;
using System.Linq;
using System.Collections.Generic;
using DynamicFilter.App.Attributes;

namespace DynamicFilter.App.Models
{
    public class FilterModelGenerator
    {
        private Type _forType;
        public List<FilterModel> Filters { get; private set; }

        public void GenerateFilterModel(ProductFilterModel model)
        {
            var classAttributes = typeof(ProductFilterModel).GetCustomAttributes(typeof(FilterForAttribute), false);
            if (!(classAttributes?.Any() == true))
                throw new Exception("Filter not applied"); //TODO: Create custom FilterNotAppliedException

            var filterForAttribute = classAttributes.First() as FilterForAttribute;
            _forType = filterForAttribute.ForType;

            var props = model.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attribute = prop.GetCustomAttributes(typeof(FilterMethodAttribute), false);
                if (!(attribute?.Any() == true))
                    continue;
                var methodAttribute = attribute.First() as FilterMethodAttribute;
                var filter = new FilterModel();
                filter.MethodName = methodAttribute.MethodName;
                filter.PropertyName = methodAttribute.PropertyName ?? prop.Name;
                filter.Value = prop.GetValue(model);
                filter.ValueType = _forType.GetProperty(filter.PropertyName).PropertyType;
                if (filter.IsValid())
                    continue;
                if (Filters == null)
                    Filters = new List<FilterModel>();
                Filters.Add(filter);
            }

        }
    }
}
