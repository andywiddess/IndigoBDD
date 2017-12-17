using System;
using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.PerformanceCounters
{
    /// <summary>
    /// This class actually wraps the .Net Performance Counter
    /// </summary>
    public class InstancePerformanceCounter :
        Counter
    {
        bool _disposed;
        PerformanceCounter _pcs;

        public InstancePerformanceCounter(string categoryName, string name, string instanceName)
        {
            _pcs = new PerformanceCounter(categoryName, name, instanceName, false)
                {
                    RawValue = 0
                };
        }

        public string Name
        {
            get { return _pcs.CounterName; }
        }

        public string InstanceName
        {
            get { return _pcs.InstanceName; }
        }

        public string CategoryName
        {
            get { return _pcs.CategoryName; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Increment()
        {
            _pcs.Increment();
        }

        public virtual void IncrementBy(long val)
        {
            _pcs.IncrementBy(val);
        }

        public virtual void Set(long val)
        {
            _pcs.RawValue = val;
        }

        public void Close()
        {
            if (_pcs != null)
            {
                _pcs.RemoveInstance();
                _pcs.Close();
                _pcs = null;
            }
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                Close();

            _disposed = true;
        }

        ~InstancePerformanceCounter()
        {
            Dispose(false);
        }
    }
}