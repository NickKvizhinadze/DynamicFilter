using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Extentions
{
    public static class TypeExtentions
    {
        public static bool IsObjectNullable(this Type source)
        {
            return source.IsGenericType && source.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetTypeFromNullable(this Type source)
        {
            return source.GetGenericArguments()[0];
        }

        public static Type GetTypeIfNullable(this Type source)
        {
            if (source.IsObjectNullable())
            {
                return source.GetGenericArguments()[0];
            }
            return source;
        }
    }
}
