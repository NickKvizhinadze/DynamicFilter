using System.Linq;

namespace DynamicFilter.Extentions
{
    public static class FilterHelper
    {
        public static IQueryable<TList> Filter<TFilter, TList>(TFilter filter, IQueryable<TList> list)
        {
            //Filter Model Generator
            var filterGenerator = new FilterModelGenerator<TFilter>();
            filterGenerator.GenerateFilterModel(filter);

            //Query Generator

            var queryGenerator = new QueryGenerator<TList>();

            //Filter Parameter
            foreach (var item in filterGenerator.Filters)
            {
                queryGenerator = (QueryGenerator<TList>)
                                typeof(QueryGenerator<TList>)
                                .GetMethod(item.MethodName)
                                .Invoke(queryGenerator, new[] { item });

                queryGenerator.AddFilter();
            }

            return queryGenerator.ApplyFilter(list);
        }
    }
}
