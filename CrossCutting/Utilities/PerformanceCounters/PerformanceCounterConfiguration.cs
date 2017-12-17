using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    /// <summary>
    /// This class encapsulates the data needed to create a counter in Windows
    /// </summary>
    public class PerformanceCounterConfiguration
    {
        readonly PerformanceCounterType _counterType;
        readonly string _help;
        readonly string _name;

        public PerformanceCounterConfiguration(string name, string help, PerformanceCounterType counterType)
        {
            _name = name;
            _help = help;
            _counterType = counterType;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Help
        {
            get { return _help; }
        }

        public static implicit operator CounterCreationData(PerformanceCounterConfiguration counterConfiguration)
        {
            return new CounterCreationData(counterConfiguration._name, counterConfiguration._help, counterConfiguration._counterType);
        }
    }
}