using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using DynamicFilter.App.Models;

namespace DynamicFilter.App
{
    public class QueryGenerator<T>
    {
        #region Fields
        private readonly List<FilterModel> _filter;
        private readonly IQueryable<T> _data;
        private readonly ParameterExpression _parameter;
        private Expression _body;
        private Expression _tempBody;
        private MethodCallExpression _whereCall;

        #endregion

        #region Constructor
        public QueryGenerator(List<FilterModel> filter, IQueryable<T> data)
        {
            _parameter = Expression.Parameter(typeof(T), "object");
            _filter = filter;
            _data = data;
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
            //_whereCall = Expression.Call(
            //   typeof(Queryable),
            //   "Where",
            //   new Type[] { _data.ElementType },
            //   _data.Expression,
            //   Expression.Lambda<Func<T, bool>>(_body, new ParameterExpression[] { _parameter })
            //   );
            return this;
        }


        public IQueryable<T> ApplyFilter()
        {
            _whereCall = Expression.Call(
               typeof(Queryable),
               "Where",
               new Type[] { _data.ElementType },
               _data.Expression,
               Expression.Lambda<Func<T, bool>>(_body, new ParameterExpression[] { _parameter })
               );
            return _data.Provider.CreateQuery<T>(_whereCall);
        }

        #endregion

        #region Private Methods
        public FilterModel GetFilterByPropertyName(string propertyName)
        {
            return _filter.FirstOrDefault(f => f.PropertyName == propertyName);
        }

        #endregion

    }
}
