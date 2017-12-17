using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1591

// code created and reused from sepura dispatcher client

namespace Indigo.CrossCutting.Utilities
{
    public class ConcurrentWorkQueue<T> where T : class
    {
        #region WorkerThread
        private class WorkerThread
        {
            private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkerThread));

            public delegate void WorkerDelegate();

            private WorkerDelegate _workerDelegate;

            private int _delay;
            private Thread _thread;
            private string _threadName;

            private bool _threadShouldStop;
            private AutoResetEvent _wakeUpEvent;

            private AutoResetEvent _threadStartedEvent;
            private AutoResetEvent _threadStoppedEvent;

            public WorkerThread(string threadName, int delay, WorkerDelegate workerDelegate)
            {
                _workerDelegate = workerDelegate;

                _delay = delay;
                _threadName = threadName;

                _threadShouldStop = false;
                _wakeUpEvent = new AutoResetEvent(false);

                _threadStartedEvent = new AutoResetEvent(false);
                _threadStoppedEvent = new AutoResetEvent(false);
            }

            public bool IsActive
            { get { return !_threadShouldStop; } }

            public void StartThread()
            {
                lock (this)
                {
                    if (_thread != null) return;

                    _threadShouldStop = false;
                    _wakeUpEvent.Reset();
                    _threadStartedEvent.Reset();
                    _threadStoppedEvent.Reset();
                    _thread = new Thread(WorkerThreadLoop);
                    _thread.Start();
                    _threadStartedEvent.WaitOne();
                }
            }

            public void StopThread()
            {
                StopThread(1000);
            }

            public void StopThread(int timeout)
            {
                lock (this)
                {
                    if (_thread == null) return;

                    _threadShouldStop = true;
                    _wakeUpEvent.Set();
                    if (!_threadStoppedEvent.WaitOne(timeout))
                    {
                        log.WarnFormat("StopThread: unable to stop thread {0} - aborting it", _threadName);
                        _thread.Abort();
                    }
                    _thread = null;
                    _wakeUpEvent.Close();
                    _threadStartedEvent.Close();
                    _threadStoppedEvent.Close();
                }
            }

            public void WakeUp()
            {
                if (IsThreadAvailableForWork())
                {
                    _wakeUpEvent.Set();
                }
            }

            #region Support

            private void WorkerThreadLoop()
            {
                _threadStartedEvent.Set();
                log.DebugFormat("Thread \"{0}\" started", _threadName);

                while (!_threadShouldStop)
                {
                    _workerDelegate();
                    if (_threadShouldStop) break;
                    _wakeUpEvent.WaitOne(_delay);
                }

                log.DebugFormat("Thread \"{0}\" stopped", _threadName);
                _threadStoppedEvent.Set();
            }

            private bool IsThreadAvailableForWork()
            {
                if (_thread == null) return false;

                switch (_thread.ThreadState)
                {
                    case ThreadState.Aborted:
                    case ThreadState.AbortRequested:
                    case ThreadState.Stopped:
                    case ThreadState.StopRequested:
                        return false;
                    default:
                        return true;
                }
            }

            #endregion
        }
        #endregion

        public delegate void WorkDelegate(T workItem);

        private WorkDelegate _workDelegate;
        private ConcurrentQueue<T> _eventQueue;

        private WorkerThread _workerThread;

        public ConcurrentWorkQueue(string name, WorkDelegate workDelegate)
        {
            _workDelegate = workDelegate;

            _eventQueue = new ConcurrentQueue<T>();
            _workerThread = new WorkerThread(name, 10000, ProcessEventQueue);
        }

        public bool IsActive
        { get { return _workerThread.IsActive; } }

        public void StartQueue()
        {
            _workerThread.StartThread();
        }

        public void StopQueue()
        {
            _workerThread.StopThread();
        }

        public void EnqueueWork(T workItem)
        {
            _eventQueue.Enqueue(workItem);
            _workerThread.WakeUp();
        }

        private T DequeueWork()
        {
            T result;
            if (_eventQueue.TryDequeue(out result))
            {
                return result;
            }
            else
            {
                return null;
            }

        }

        private void ProcessEventQueue()
        {
            T workItem;

            while (_workerThread.IsActive && (workItem = DequeueWork()) != null)
            {
                _workDelegate(workItem);
            }
        }
    }
}
