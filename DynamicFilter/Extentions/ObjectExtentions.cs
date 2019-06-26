using System.Collections;

namespace DynamicFilter.Extentions
{
    internal static class ObjectExtentions
    {
        internal static bool IsNotNullOrEmptyArray(this object source)
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
