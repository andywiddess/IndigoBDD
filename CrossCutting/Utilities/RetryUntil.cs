using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Class encapsulating injected code which will retry until the given timeout, at given intervals.
    /// 
    /// Create the instance of the class then call ExecuteRetryLoop and examine the return value to see if the DoWorkDelegate eventually got executed.
    /// </summary>
    public class RetryUntil
    {
        /// <summary>
        /// Retry until this limit is reached since the first attempt. Defaults to 5 seconds.
        /// </summary>
        public TimeSpan RetryUntilLimit = new TimeSpan(0,0,5);
        /// <summary>
        /// Retry int the following intervals (using thread.sleep). Defaults to 250ms.
        /// </summary>
        public TimeSpan RetryInterval = new TimeSpan(0,0,0,0,250);

        /// <summary>
        /// The date/time this retry until block was started
        /// </summary>
        protected readonly DateTime StartDateTime;

        /// <summary>
        /// The delegate to be executed. If it returns FALSE the block will be retried until the retry limit is reached in intervals of retryinterval.
        /// </summary>
        /// <returns></returns>
        public delegate bool DoWork();

        /// <summary>
        /// The instance of the DoWork delegate to execute.
        /// </summary>
        public DoWork DoWorkDelegate = null;

        /// <summary>
        /// A list of Exception Types which, if encountered, will be treated as Retry events and swallowed. 
        /// </summary>
        public List<Type> ExpectedExceptionTypes = null;

        public RetryUntil(DoWork doWorkDelegate, params Type[] expectedExceptionTypes)
        {
            this.ExpectedExceptionTypes = new List<Type>(expectedExceptionTypes);
            this.DoWorkDelegate = doWorkDelegate;
            this.StartDateTime = DateTime.Now;
        }

        public RetryUntil(DoWork doWorkDelegate, TimeSpan retryInterval, params Type[] expectedExceptionTypes)
        {
            this.ExpectedExceptionTypes= new List<Type>(expectedExceptionTypes);
            this.RetryInterval = retryInterval;
            this.DoWorkDelegate = doWorkDelegate;
            this.StartDateTime = DateTime.Now;
        }

        public RetryUntil(DoWork doWorkDelegate, TimeSpan retryInterval, TimeSpan retryUntil, params Type[] expectedExceptionTypes)
        {
            this.ExpectedExceptionTypes = new List<Type>(expectedExceptionTypes);
            this.DoWorkDelegate = doWorkDelegate;
            this.StartDateTime = DateTime.Now;
            this.RetryUntilLimit = retryUntil;
        }

        /// <summary>
        /// Carry out the execution logic
        /// </summary>
        /// <returns>TRUE if the DoWorkDelegate was successfully run within the timeout limit</returns>
        public virtual bool ExecuteRetryLoop()
        {
            bool retry = true;
            bool executedSuccessfully = false;
            while (retry && ((DateTime.Now - this.StartDateTime) < this.RetryUntilLimit))
            {
                try
                {
                    retry = !this.DoWorkDelegate();
                    if (retry)
                    {
                        System.Threading.Thread.Sleep(this.RetryInterval);
                    }
                    else
                    {
                        executedSuccessfully = true;
                    }
                }
                catch (Exception e)
                {
                    // If the exception type does not exactly match the type specified, and the consumer doesn't want inherited exceptions treated similarly,
                    // rethrow the exception.
                    if (this.ExpectedExceptionTypes == null)
                    {
                        throw;
                    }

                    if (! this.ExpectedExceptionTypes.Contains(e.GetType()))
                    {
                        throw;
                    }
                    retry = true;
                }
                if (retry)
                {
                    System.Threading.Thread.Sleep(this.RetryInterval);
                }
            }

            return executedSuccessfully;
        }
    }

}
