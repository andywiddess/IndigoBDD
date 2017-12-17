using System;

namespace Indigo.CrossCutting.Utilities.Caching
{
    public delegate TValue MissingValueProvider<in TKey, out TValue>(TKey key);
}