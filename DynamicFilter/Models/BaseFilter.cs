using DynamicFilter.ValidationBuilder;
using System;
using System.Collections.Generic;

namespace DynamicFilter.Models
{
    public abstract class BaseFilter
    {
        protected Dictionary<string, Func<object, bool>> _predicates = new Dictionary<string, Func<object, bool>>();

        protected FilterValidationBuilder ValidationBuilder { get => new FilterValidationBuilder(_predicates); }


        public virtual void Configure()
        {
        }

        internal Dictionary<string, Func<object, bool>> GetPredicates()
        {
            return _predicates;
        }
    }
}
