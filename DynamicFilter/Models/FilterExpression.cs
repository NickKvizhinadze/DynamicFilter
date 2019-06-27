using DynamicFilter.Enums;
using System.Linq.Expressions;

namespace DynamicFilter.Models
{
    public class FilterExpression
    {
        public FilterExpression(Expression expression, ConditionalOperators? method = null)
        {
            Method = method;
            Expression = expression;
        }

        public ConditionalOperators? Method { get; set; }
        public Expression Expression { get; set; }
    }
}
