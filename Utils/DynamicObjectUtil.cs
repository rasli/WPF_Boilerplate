using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DynamicObjectUtil
    {
        public static List<T> GetListOfType<T>(T dyn, int? count = null)
        {
            return count.HasValue ? new List<T>(count.Value) : new List<T>();
        }

        public static T CastTo<T>(object x, T dyn)
        {
            return (T)x;
        }
        public static void DynamicUsing(object resource, Action action)
        {
            try
            {
                action();
            }
            finally
            {
                IDisposable d = resource as IDisposable;
                if (d != null)
                    d.Dispose();
            }
        }
    }
}
