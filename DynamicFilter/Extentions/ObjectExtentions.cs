using System;
using System.Collections;
using System.Linq;

namespace DynamicFilter.Extentions
{
    public static class ObjectExtentions
    {
        public static bool IsNotNullOrEmptyArray(this object source)
        {
            if (source.GetType().GetInterface("IEnumerable") != null)
            {
                var list = source as IList;
                return list == null || list.Count == 0;
            }

            return false;
        }

    }
}
