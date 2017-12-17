using System;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    public class NullPerformanceCounter :
        Counter
    {
        public string Name
        {
            get { return "null"; }
        }

        public void Increment()
        {
        }

        public void IncrementBy(long val)
        {
        }

        public void Set(long val)
        {
        }

        public void Dispose()
        {
        }
    }
}