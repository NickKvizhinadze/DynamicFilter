using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using DynamicFilter.Models;
using DynamicFilter.Extentions;
using DynamicFilter.Enums;

namespace DynamicFilter
{
    internal class QueryGenerator<T>
    {
        #region Fields
        private readonly ParameterExpression _parameter;
        private Expression _body;
        //private Expression body;
        private List<FilterExpression> _tempBodies = new List<FilterExpression>();
        private MethodCallExpression _whereCall;

        #endregion

        #region Constructor
        internal QueryGenerator()
        {
            _parameter = Expression.Parameter(typeof(T), "item");
        }
        #endregion


        #region Methods

        #region Filter Methods
        internal QueryGenerator<T> StringContains(FilterModel filter)
        {
            StringContains(filter, false);
            return this;
        }

        internal QueryGenerator<T> Contains(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));

            MethodInfo method = filter.ValueType.GetMethod("Contains", BindingFlags.Public | BindingFlags.Instance);

            Expression right = Expression.Constant(filter.Value, filter.ValueType);

            var body = Expression.Call(right, method, left);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> HasValueAndContains(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = HasValue(filter, left);

            MethodInfo method = filter.ValueType.GetMethod("Contains", BindingFlags.Public | BindingFlags.Instance);

            if (filter.PropertyType.IsObjectNullable())
            {
                var newType = filter.PropertyType.GetTypeFromNullable();
                left = Expression.Convert(left, newType);
            }

            Expression right = Expression.Constant(filter.Value, filter.ValueType);

            Expression e2 = Expression.Call(right, method, left);
            var body = Expression.AndAlso(e1, e2);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> Equal(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType), filter.PropertyType);
            var body = Expression.Equal(left, right);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> HasValueAndEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = HasValue(filter, left);

            var propertyType = filter.PropertyType.GetTypeIfNullable();

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, propertyType), filter.PropertyType);
            Expression e2 = Expression.Equal(left, right);

            var body = Expression.AndAlso(e1, e2);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> GreaterThan(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = null;
            if (filter.PropertyType.IsObjectNullable())
            {
                e1 = HasValue(filter, left);
                filter.PropertyType = filter.PropertyType.GetTypeFromNullable();
                left = Expression.Convert(left, filter.PropertyType);
            }

            Expression right = GetConstant(filter);

            Expression body;
            if (e1 != null)
            {
                Expression e2 = Expression.GreaterThan(left, right);
                body = Expression.AndAlso(e1, e2);
            }
            else
                body = Expression.GreaterThan(left, right);

            _tempBodies.Add(new FilterExpression(body));

            return this;
        }

        internal QueryGenerator<T> GreaterThanOrEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = null;
            if (filter.PropertyType.IsObjectNullable())
            {
                e1 = HasValue(filter, left);
                filter.PropertyType = filter.PropertyType.GetTypeFromNullable();
                left = Expression.Convert(left, filter.PropertyType);
            }

            Expression right = GetConstant(filter);

            Expression body;
            if (e1 != null)
            {
                Expression e2 = Expression.GreaterThanOrEqual(left, right);
                body = Expression.AndAlso(e1, e2);
            }
            else
                body = Expression.GreaterThanOrEqual(left, right);

            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> LessThan(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = null;
            if (filter.PropertyType.IsObjectNullable())
            {
                e1 = HasValue(filter, left);
                filter.PropertyType = filter.PropertyType.GetTypeFromNullable();
                left = Expression.Convert(left, filter.PropertyType);
            }

            Expression right = GetConstant(filter);

            Expression body;
            if (e1 != null)
            {
                Expression e2 = Expression.LessThan(left, right);
                body = Expression.AndAlso(e1, e2);
            }
            else
                body = Expression.LessThan(left, right);

            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> LessThanOrEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = null;

            if (filter.PropertyType.IsObjectNullable())
            {
                e1 = HasValue(filter, left);
                filter.PropertyType = filter.PropertyType.GetTypeFromNullable();
                left = Expression.Convert(left, filter.PropertyType);
            }

            Expression right = GetConstant(filter);

            Expression body;
            if (e1 != null)
            {
                Expression e2 = Expression.LessThanOrEqual(left, right);
                body = Expression.AndAlso(e1, e2);
            }
            else
                body = Expression.LessThanOrEqual(left, right);

            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> IsNotNull(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression body = HasValue(filter, left);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }
        internal QueryGenerator<T> IsNull(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression body = HasNotValue(filter, left);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        internal QueryGenerator<T> Condition(ConditionalOperators conditionOperator)
        {
            if (_tempBodies.Any())
            {
                var expression = _tempBodies.Last();
                expression.Method = conditionOperator;
            }

            return this;
        }


        #endregion

        internal QueryGenerator<T> AddFilter()
        {
            Expression body = GenerateBody();

            if (body == null)
                return this;

            if (_body == null)
                _body = body;
            else
                _body = Expression.AndAlso(_body, body);

            _tempBodies = new List<FilterExpression>();
            return this;
        }

        internal IQueryable<T> ApplyFilter(IQueryable<T> data)
        {
            _whereCall = Expression.Call(
               typeof(Queryable),
               "Where",
               new Type[] { data.ElementType },
               data.Expression,
               Expression.Lambda<Func<T, bool>>(_body, new ParameterExpression[] { _parameter })
               );
            _body = null;
            return data.Provider.CreateQuery<T>(_whereCall);
        }
        
        #endregion

        #region Private Methods

        private Expression HasValue(FilterModel filter, Expression left)
        {
            Expression right = Expression.Constant(null, filter.PropertyType);
            return Expression.NotEqual(left, right);
        }

        private Expression HasNotValue(FilterModel filter, Expression left)
        {
            Expression right = Expression.Constant(null, filter.PropertyType);
            return Expression.Equal(left, right);
        }

        private Expression GenerateBody()
        {
            Expression result = null;
            if (_tempBodies != null && _tempBodies.Any())
            {
                foreach (var body in _tempBodies)
                {
                    if (result == null)
                        result = body.Expression;
                    else
                    {
                        switch (body.Method)
                        {
                            case ConditionalOperators.Or:
                                result = Expression.OrElse(result, body.Expression);
                                break;
                            case ConditionalOperators.And:
                                result = Expression.AndAlso(result, body.Expression);
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private QueryGenerator<T> StringContains(FilterModel filter, bool matchCase)
        {
            if (filter.PropertyType != typeof(string))
                throw new TypeLoadException("Incorrect type");

            Expression propertyExpression = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression left = !matchCase ? Expression.Call(propertyExpression, "ToLower", null) : propertyExpression;

            MethodInfo method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            var constant = matchCase ? filter.Value : filter.Value.ToString().ToLower();
            Expression right = Expression.Constant(constant, typeof(string));

            var body = Expression.Call(left, method, right);
            _tempBodies.Add(new FilterExpression(body));
            return this;
        }

        private Expression GetConstant(FilterModel filter)
        {
            Expression right;
            if (filter.PropertyType == typeof(DateTime))
            {
                var value = (DateTime)filter.Value;
                Expression<Func<DateTime>> dateConstant = () => value;
                right = dateConstant.Body;
            }
            else
                right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType));
            return right;
        }

        #endregion
    }
}
