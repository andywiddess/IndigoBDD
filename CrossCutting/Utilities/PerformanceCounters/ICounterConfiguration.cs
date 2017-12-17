using System;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    public interface ICounterConfiguration
    {
        void Register<TCounterCategory>() where TCounterCategory : CounterCategory;
    }
}