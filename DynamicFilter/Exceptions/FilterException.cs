using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Exceptions
{
    [Serializable]
    public class FilterException: Exception
    {
        public FilterException()
        {
        }
        public FilterException(string message) : base(message)
        {
        }
    }
}
