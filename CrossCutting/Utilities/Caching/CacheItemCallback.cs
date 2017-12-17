using System;

namespace Indigo.CrossCutting.Utilities.Caching
{
    public delegate void CacheItemCallback<in TKey, in TValue>(TKey key, TValue value);
}