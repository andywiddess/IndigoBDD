using System;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    /// <summary>
    /// A class used for type scanning
    /// </summary>
    public interface CounterRegistry
    {
        void Register(ICounterConfiguration cfg);
    }
}