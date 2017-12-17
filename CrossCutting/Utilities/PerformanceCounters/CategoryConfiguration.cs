using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    public class CategoryConfiguration
    {
        public CategoryConfiguration()
        {
            Counters = new List<PerformanceCounterConfiguration>();
        }

        public string Name { get; set; }
        public string Help { get; set; }
        public List<PerformanceCounterConfiguration> Counters { get; private set; }
    }
}