using System;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    /// <summary>
    /// How to configure/create a counter repository
    /// </summary>
    public static class CounterRepositoryConfigurator
    {
        public static CounterRepository New(Action<ICounterConfiguration> action)
        {
            var cfg = new WindowsCounterConfiguration();

            action(cfg);

            return cfg.BuildRepository();
        }
    }
}