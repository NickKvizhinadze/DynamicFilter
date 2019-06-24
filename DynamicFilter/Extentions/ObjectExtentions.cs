using System;

namespace DynamicFilter.Extentions
{
    public static class ObjectExtentions
    {
        public static bool IsNotNullOrEmptyArray(this object source)
        {
            return (source.GetType().IsArray || source.GetType().GetInterface("IEnumerable") != null) && (source as Array) == null;
        }
        
    }
}
