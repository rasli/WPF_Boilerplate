using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class EnumerableExtender
    {
        public static void ForEach<T>(this IEnumerable<T> _this, Action<T> callback)
        {
            foreach (T entry in _this)
            {
                callback(entry);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var hash = new HashSet<TKey>();
            return source.Where(p => hash.Add(keySelector(p)));
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> toIntersect, Func<TSource, TSource, bool> keySelector)
        {
            return source.Intersect(toIntersect, new ActionEqualityComparator<TSource>(keySelector));
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> toRemain, Func<TSource, TSource, bool> keySelector)
        {
            return source.Except(toRemain, new ActionEqualityComparator<TSource>(keySelector));
        }

        public static int Count(this IEnumerable source)
        {
            var col = source as ICollection;
            if (col != null)
                return col.Count;

            int c = 0;
            var e = source.GetEnumerator();
            DynamicObjectUtil.DynamicUsing(e, () => {
                while (e.MoveNext())
                    c++;
            });

            return c;
        }
    }
    public class ActionEqualityComparator<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> comparer;

        public ActionEqualityComparator(Func<T, T, bool> comparer)
        {
            this.comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return comparer(x, y);
        }

        public int GetHashCode(T codeh)
        {
            return 1;
        }
    }
}
