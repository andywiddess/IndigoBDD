using System;
using System.Linq;
using System.Reflection;
using Indigo.CrossCutting.Utilities.Caching;

namespace Indigo.CrossCutting.Utilities.Reflection
{
    public class FastPropertyCache<T> :
        AbstractCacheDecorator<string, FastProperty<T>>
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public FastPropertyCache()
            : base(CreatePropertyCache())
        {
        }

        static ConcurrentCache<string, FastProperty<T>> CreatePropertyCache()
        {
            return new ConcurrentCache<string, FastProperty<T>>(typeof(T).GetProperties(Flags)
                                                                    .Select(x => new FastProperty<T>(x, Flags))
                                                                    .ToDictionary(x => x.Property.Name));
        }

        public void Each(T instance, Action<object> action)
        {
            Each(property => action(property.Get(instance)));
        }
    }
}