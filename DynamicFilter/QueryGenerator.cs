using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using DynamicFilter.Models;
using DynamicFilter.Extentions;

namespace DynamicFilter
{
    internal class QueryGenerator<T>
    {
        #region Fields
        private readonly ParameterExpression _parameter;
        private Expression _body;
        private Expression _tempBody;
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
        internal QueryGenerator<T> StringContains(FilterModel filter, bool matchCase)
        {
            if (filter.PropertyType != typeof(string))
                throw new TypeLoadException("Incorrect type");

            Expression propertyExpression = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression left = !matchCase ? Expression.Call(propertyExpression, "ToLower", null) : propertyExpression;

            MethodInfo method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            var constant = matchCase ? filter.Value : filter.Value.ToString().ToLower();
            Expression right = Expression.Constant(constant, typeof(string));

            _tempBody = Expression.Call(left, method, right);
            return this;
        }

        internal QueryGenerator<T> Contains(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));

            MethodInfo method = filter.ValueType.GetMethod("Contains", BindingFlags.Public | BindingFlags.Instance);

            Expression right = Expression.Constant(filter.Value, filter.ValueType);

            _tempBody = Expression.Call(right, method, left);
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
            _tempBody = Expression.AndAlso(e1, e2);
            return this;
        }

        internal QueryGenerator<T> Equal(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType), filter.PropertyType);
            _tempBody = Expression.Equal(left, right);
            return this;
        }

        internal QueryGenerator<T> HasValueAndEqual(FilterModel filter)
        {
            Expression left = Expression.Property(_parameter, typeof(T).GetProperty(filter.PropertyName));
            Expression e1 = HasValue(filter, left);

            var propertyType = filter.PropertyType.GetTypeIfNullable();

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, propertyType), filter.PropertyType);
            Expression e2 = Expression.Equal(left, right);

            _tempBody = Expression.AndAlso(e1, e2);
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

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType));

            if (e1 != null)
            {
                Expression e2 = Expression.GreaterThan(left, right);
                _tempBody = Expression.AndAlso(e1, e2);
            }
            else
                _tempBody = Expression.GreaterThan(left, right);

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

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType));

            if (e1 != null)
            {
                Expression e2 = Expression.GreaterThanOrEqual(left, right);
                _tempBody = Expression.AndAlso(e1, e2);
            }
            else
                _tempBody = Expression.GreaterThanOrEqual(left, right);
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

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType));

            if (e1 != null)
            {
                Expression e2 = Expression.LessThan(left, right);
                _tempBody = Expression.AndAlso(e1, e2);
            }
            else
                _tempBody = Expression.LessThan(left, right);
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

            Expression right = Expression.Constant(Convert.ChangeType(filter.Value, filter.PropertyType));
            if (e1 != null)
            {
                Expression e2 = Expression.LessThanOrEqual(left, right);
                _tempBody = Expression.AndAlso(e1, e2);
            }
            else
                _tempBody = Expression.LessThanOrEqual(left, right);

            return this;
        }

        #endregion

        internal QueryGenerator<T> AddFilter()
        {
            if (_tempBody == null)
                throw new Exception("Body is null");

            if (_body == null)
                _body = _tempBody;
            else
                _body = Expression.AndAlso(_body, _tempBody);
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
            return data.Provider.CreateQuery<T>(_whereCall);
        }

        #endregion

        #region Private Methods

        private Expression HasValue(FilterModel filter, Expression left)
        {
            Expression right1 = Expression.Constant(null, filter.PropertyType);
            Expression e1 = Expression.NotEqual(left, right1);
            return e1;
        }
        #endregion
    }
}
