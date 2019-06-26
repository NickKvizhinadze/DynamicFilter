using System.Linq;
using System.Reflection;
using DynamicFilter.Models;

namespace DynamicFilter.Helpers
{
    public static class FilterHelper
    {
        public static IQueryable<TList> Filter<TFilter, TList>(TFilter filter, IQueryable<TList> list) where TFilter: BaseFilter
        {
            //Filter Model Generator
            var filterGenerator = new FilterModelGenerator<TFilter>();
            filterGenerator.GenerateFilterModel(filter);

            //Query Generator

            var queryGenerator = new QueryGenerator<TList>();

            //Filter Parameter
            if (filterGenerator.Filters == null)
                return list;
                        
            foreach (var item in filterGenerator.Filters)
            {
                queryGenerator = (QueryGenerator<TList>)
                                typeof(QueryGenerator<TList>)
                                .GetMethod(item.MethodName, BindingFlags.NonPublic | BindingFlags.Instance)
                                .Invoke(queryGenerator, new[] { item });

                queryGenerator.AddFilter();
            }

            return queryGenerator.ApplyFilter(list);
        }
    }
}
