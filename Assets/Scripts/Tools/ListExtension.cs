using System.Collections.Generic;

namespace Tools
{
    public static class ListExtension
    {
        public static T Shift<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var item = list[0];
                list.RemoveAt(0);
                return item;
            }

            return default;
        }
        
    }
}