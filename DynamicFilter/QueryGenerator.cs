using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using DynamicFilter.Models;

namespace DynamicFilter
{
    public class QueryGenerator<T>
    {
        #region Fields
        private readonly ParameterExpression _parameter;
        private Expression _body;
        private Expression _tempBody;
        private MethodCallExpression _whereCall;

        #endregion

        #region Constructor
        public QueryGenerator()
        {
            _parameter = Expression.Parameter(typeof(T), "item");
        }
        #endregion


        #region Methods

        #region Filter Methods
        public QueryGenerator<T> StringContains(FilterModel filter, bool matchCase)
        {
            if (filter.ValueType != typeof(string))
                throw new TypeLoadException("Incorrect type");

            Expression propertyExpression = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression left = !matchCase ? Expression.Call(propertyExpression, "ToLower", null) : propertyExpression;

            MethodInfo method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            var constant = matchCase ? filter.Value : filter.Value.ToString().ToLower();
            Expression right = Expression.Constant(constant, typeof(string));

            _tempBody = Expression.Call(left, method, right);
            return this;
        }

        public QueryGenerator<T> Contains(FilterModel filter)
        {
            Expression propertyExpression = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression left = propertyExpression;

            var listType = typeof(List<>).MakeGenericType(new[] { filter.ValueType });
            MethodInfo method = listType.GetMethod("Contains", BindingFlags.Public | BindingFlags.Instance);

            Expression right = Expression.Constant(filter.Value, listType);

            _tempBody = Expression.Call(right, method, left);
            return this;
        }

        public QueryGenerator<T> Equal(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.ValueType), filter.ValueType);
            _tempBody = Expression.Equal(left, right);
            return this;
        }

        public QueryGenerator<T> GreaterThen(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(filter.Value);
            _body = Expression.GreaterThan(left, right);
            return this;
        }

        public QueryGenerator<T> GreaterThenOrEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(filter.Value);
            _body = Expression.GreaterThanOrEqual(left, right);
            return this;
        }

        public QueryGenerator<T> LessThan(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(filter.Value);
            _body = Expression.LessThan(left, right);
            return this;
        }

        public QueryGenerator<T> LessThanOrEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(filter.Value);
            _body = Expression.LessThanOrEqual(left, right);
            return this;
        }

        #endregion

        public QueryGenerator<T> AddFilter()
        {
            if (_tempBody == null)
                throw new Exception("Body is null");

            if (_body == null)
                _body = _tempBody;
            else
                _body = Expression.AndAlso(_body, _tempBody);
            return this;
        }


        public IQueryable<T> ApplyFilter(IQueryable<T> data)
        {
            _whereCall = Expression.Call(
               typeof(Queryable),
               "Where",
               new Type[] { data.ElementType },
               data.Expression,
               Expression.Lambda<Func<T, bool>>(_body, new ParameterExpression[] { _parameter })
               );
            return data.Provider.CreateQuery<T>(_whereCall);
        }

        #endregion
    }
}
