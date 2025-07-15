using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public static class ObjectHelper
    {
        public static TDest Map<TSource, TDest>(TSource source) where TDest : new()
        {
            var dest = new TDest();
            if (source == null) return dest;

            var sourceProps = typeof(TSource).GetProperties();
            var destProps = typeof(TDest).GetProperties();

            foreach (var sProp in sourceProps)
            {
                var dProp = destProps.FirstOrDefault(p => p.Name == sProp.Name && p.PropertyType == sProp.PropertyType);
                if (dProp != null && dProp.CanWrite)
                {
                    var value = sProp.GetValue(source);
                    dProp.SetValue(dest, value);
                }
            }
            return dest;
        }

        public static List<TDest> MapList<TSource, TDest>(List<TSource> list) where TDest : new()
        {
            return list.Select(Map<TSource, TDest>).ToList();
        }
    }

}
