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
                    queryGenerator = (QueryGenerator<TList>)
                                    typeof(QueryGenerator<TList>)
                                    .GetMethod(item.MethodName, BindingFlags.NonPublic | BindingFlags.Instance)
                                    .Invoke(queryGenerator, new[] { item });
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
    }
}
