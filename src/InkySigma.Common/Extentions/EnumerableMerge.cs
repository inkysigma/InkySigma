using System.Collections.Generic;
using System.Linq;

namespace InkySigma.Common.Extentions
{
    public static class EnumerableMerge
    {
        public static IEnumerable<T> CarefullyMerge<T>(this IEnumerable<T> current, IEnumerable<T> toMerge)
        {
            var enumerable = current.ToList();
            foreach (var i in toMerge)
            {
                if (enumerable.Contains(i))
                    continue;
                enumerable.Add(i);
            }
            return enumerable;
        }
    }
}