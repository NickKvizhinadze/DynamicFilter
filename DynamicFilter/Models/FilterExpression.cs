using DynamicFilter.Enums;
using System.Linq.Expressions;

namespace DynamicFilter.Models
{
    internal class FilterExpression
    {
        internal FilterExpression(Expression expression, ConditionalOperators? method = null)
        {
            Method = method;
            Expression = expression;
        }

        internal ConditionalOperators? Method { get; set; }
        internal Expression Expression { get; set; }
    }
}
