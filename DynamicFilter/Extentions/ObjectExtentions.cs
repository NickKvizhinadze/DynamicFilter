using System.Collections;

namespace DynamicFilter.Extentions
{
    internal static class ObjectExtentions
    {
        internal static bool IsNullOrEmptyArray(this object source)
        {
            if (source.GetType() != typeof(string) && source.GetType().GetInterface("IEnumerable") != null)
            {
                var list = source as IList;
                return list == null || list.Count == 0;
            }

            return false;
        }

    }
}
