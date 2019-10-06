using System;
using System.Collections.Generic;

namespace DynamicFilter.Models
{
    /// <summary>
    /// A base class for data filter
    /// </summary>
    public abstract class BaseFilter
    {
        internal Dictionary<string, Func<object, bool>> Predicates { get; } = new Dictionary<string, Func<object, bool>>();

        /// <summary>
        /// Configuration method for validation
        /// </summary>
        public virtual void Configure()
        {
        }
    }
}
