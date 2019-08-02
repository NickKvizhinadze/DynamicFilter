using System;
using System.Linq;
using System.Reflection;
using DynamicFilter.Models;

namespace DynamicFilter.Helpers
{
    public static class FilterHelper
    {
        public static IQueryable<TList> Filter<TFilter, TList>(TFilter filterModel, IQueryable<TList> list) where TFilter : BaseFilter
        {
            //Filter Model Generator
            var filterGenerator = new FilterModelGenerator<TFilter>();
            filterGenerator.GenerateFilterModel(filterModel);

            //Query Generator

            var queryGenerator = new QueryGenerator<TList>();

            //Filter Parameter
            if (filterGenerator.Filters == null)
                return list;

            var filters = filterGenerator.Filters.GroupBy(f => f.PropertyName);

            foreach (var filter in filters)
            {
                var index = 0;
                foreach (var item in filter)
                {
                    if (item.AlreadyUsed)
                        continue;

                    GenerateFilterQuery(queryGenerator, item);

                    var sameFilters = filterGenerator.Filters
                       .Where(f => f.FilterPropertyName == item.FilterPropertyName && f.PropertyName != item.PropertyName && !f.AlreadyUsed)
                       .ToList();
                    if (sameFilters.Any())
                    {
                        foreach (var sameItem in sameFilters)
                        {
                            GenerateFilterQuery(queryGenerator, sameItem);

                            if (!sameItem.ConditionalOperator.HasValue)
                                throw new Exception($"{nameof(item.FilterPropertyName)} does not have ConditionalOperator for filtering {item.PropertyName}"); // Add Custom Exception

                            queryGenerator.Condition(sameItem.ConditionalOperator.Value);
                        }
                    }

                    if (filter.Count() != 1 && index > 0 && item.ConditionalOperator.HasValue)
                    {
                        queryGenerator.Condition(item.ConditionalOperator.Value);
                    }
                    index++;
                }

                queryGenerator.AddFilter();
            }

            return queryGenerator.ApplyFilter(list);
        }

        #region Private Methods
        private static QueryGenerator<TList> GenerateFilterQuery<TList>(QueryGenerator<TList> queryGenerator, FilterModel item)
        {
            queryGenerator = (QueryGenerator<TList>)
                            typeof(QueryGenerator<TList>)
                            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                            .FirstOrDefault(x => x.Name == item.MethodName && x.IsAssembly)
                            .Invoke(queryGenerator, new[] { item });
            item.AlreadyUsed = true;
            return queryGenerator;
        }
        #endregion
    }
}
