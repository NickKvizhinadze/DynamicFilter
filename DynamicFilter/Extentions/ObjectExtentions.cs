using System.Collections;

namespace DynamicFilter.Extentions
{
    internal static class ObjectExtentions
    {
        internal static bool IsNullOrEmptyArray(this object source)
        {
            if (source is IList list)
            {
                return list.Count == 0;
            }
            return true;
        }
    }
}
