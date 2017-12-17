using System;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    public interface Counter :
        IDisposable
    {
        string Name { get; }
        void Increment();
        void IncrementBy(long value);
        void Set(long value);
    }
}