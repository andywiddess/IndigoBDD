using System;

namespace Indigo.CrossCutting.Utilities.Caching
{
    public delegate TKey KeySelector<out TKey, in TValue>(TValue value);
}