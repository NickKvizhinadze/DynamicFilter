using System.Linq.Expressions;

namespace DynamicFilter.Models
{
    public class FilterExpression
    {
        public FilterExpression(string method, Expression expression)
        {
            Method = method;
            Expression = expression;
        }

        public string Method { get; set; }
        public Expression Expression { get; set; }
    }
}
