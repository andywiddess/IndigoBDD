using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    public static class PerformanceCounters
    {
        #region Constants
        /// <summary>
        /// The rang e_ limi t_ information r_ event
        /// </summary>
        private static long RANGE_LIMIT_FOR_EVENT = 1000;
        #endregion

        #region Members
        /// <summary>
        /// The counters
        /// </summary>
        private static PerformanceCounter NumberOfAPICalls = null;
        private static PerformanceCounter NumberOfGetAPICalls = null;
        private static PerformanceCounter NumberOfPostAPICalls = null;
        private static PerformanceCounter NumberOfDeleteAPICalls = null;
        private static PerformanceCounter NumberOfPatchAPICalls = null;
        private static PerformanceCounter NumberOfLoginAPICalls = null;
        private static PerformanceCounter NumberOfLogoutAPICalls = null;
        private static PerformanceCounter NumberOfServiceErrors = null;
        private static PerformanceCounter NumberOfControllerErrors = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sets the increment API count.
        /// </summary>
        /// <value>
        /// The increment API count.
        /// </value>
        public static long IncrementAPICount
        {
            set
            {
                try
                {
                    if (NumberOfAPICalls == null)
                        Initialise();

                    NumberOfAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API Count currently {0} ", NumberOfAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment get API count.
        /// </summary>
        /// <value>
        /// The increment get API count.
        /// </value>
        public static long IncrementGetAPICount
        {
            set
            {
                try
                {
                    if (NumberOfGetAPICalls == null)
                        Initialise();

                    NumberOfGetAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfGetAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API HTTPGET Count currently {0} ", NumberOfGetAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment post API count.
        /// </summary>
        /// <value>
        /// The increment post API count.
        /// </value>
        public static long IncrementPostAPICount
        {
            set
            {
                try
                {
                    if (NumberOfPostAPICalls == null)
                        Initialise();

                    NumberOfPostAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfPostAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API HTTPPOST Count currently {0} ", NumberOfPostAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment patch API count.
        /// </summary>
        /// <value>
        /// The increment patch API count.
        /// </value>
        public static long IncrementPatchAPICount
        {
            set
            {
                try
                {
                    if (NumberOfPatchAPICalls == null)
                        Initialise();

                    NumberOfPatchAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfPatchAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API HTTPATCH Count currently {0} ", NumberOfPatchAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment delete API count.
        /// </summary>
        /// <value>
        /// The increment delete API count.
        /// </value>
        public static long IncrementDeleteAPICount
        {
            set
            {
                try
                {
                    if (NumberOfDeleteAPICalls == null)
                        Initialise();

                    NumberOfDeleteAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfDeleteAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API DELETE Count currently {0} ", NumberOfDeleteAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment login API count.
        /// </summary>
        /// <value>
        /// The increment login API count.
        /// </value>
        public static long IncrementLoginAPICount
        {
            set
            {
                try
                {
                    if (NumberOfLoginAPICalls == null)
                        Initialise();

                    NumberOfLoginAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfLoginAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API LOGIN Count currently {0} ", NumberOfLoginAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment logout API count.
        /// </summary>
        /// <value>
        /// The increment logout API count.
        /// </value>
        public static long IncrementLogoutAPICount
        {
            set
            {
                try
                {
                    if (NumberOfLogoutAPICalls == null)
                        Initialise();

                    NumberOfLogoutAPICalls.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfLogoutAPICalls.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("API LOGOUT Count currently {0} ", NumberOfLogoutAPICalls.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment service errors count.
        /// </summary>
        /// <value>
        /// The increment service errors count.
        /// </value>
        public static long IncrementServiceErrorsCount
        {
            set
            {
                try
                {
                    if (NumberOfServiceErrors == null)
                        Initialise();

                    NumberOfServiceErrors.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfServiceErrors.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("Service Error Count currently {0} ", NumberOfServiceErrors.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }

        /// <summary>
        /// Sets the increment controller errors count.
        /// </summary>
        /// <value>
        /// The increment controller errors count.
        /// </value>
        public static long IncrementControllerErrorsCount
        {
            set
            {
                try
                {
                    if (NumberOfControllerErrors == null)
                        Initialise();

                    NumberOfControllerErrors.IncrementBy(value);

                    // DO we need to send an event?
                    if ((NumberOfControllerErrors.RawValue % RANGE_LIMIT_FOR_EVENT) == 0)
                        InstrumentationProvider.FireEvent(string.Format("Controller Error Count currently {0} ", NumberOfControllerErrors.RawValue), EventType.APIRangeReached);
                }
                catch (Exception)
                {
                    // Log Exception
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialises this instance.
        /// </summary>
        private static void Initialise()
        {
            try
            {
                // If the category does not exist, create the category and exit. 
                // Perfomance counters should not be created and immediately used. 
                // There is a latency time to enable the counters, they should be created 
                // prior to executing the application that uses the counters. 
                // Execute this sample a second time to use the category. 
                SetupCategory();

                CreateCounters();
            }
            catch (Exception)
            {
                // Log Exception
            }
        }

        /// <summary>
        /// Setups the category.
        /// </summary>
        /// <returns></returns>
        private static bool SetupCategory()
        {
            bool result = false;

            try
            {
                if (PerformanceCounterCategory.Exists("Sepura"))
                    PerformanceCounterCategory.Delete("Sepura");

                CounterCreationDataCollection CCDC = new CounterCreationDataCollection();

                // Add the counter.
                CounterCreationData NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfAPICalls";
                CCDC.Add(NOI64);

                // Add the counter.
                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfGetAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfPostAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfPatchAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfDeleteAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfLoginAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfLogoutAPICalls";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfServiceErrors";
                CCDC.Add(NOI64);

                NOI64 = new CounterCreationData();
                NOI64.CounterType = PerformanceCounterType.NumberOfItems64;
                NOI64.CounterName = "NumberOfControllerErrors";
                CCDC.Add(NOI64);

                // Create the category.
                PerformanceCounterCategory.Create("Sepura",
                                                  "Performance Counter Collection for measuring internal performance of the Sepura product",
                                                  PerformanceCounterCategoryType.SingleInstance, CCDC);

                result = true;
            }
            catch (System.Security.SecurityException)
            {
                // Log Error
            }

            return result;
        }

        /// <summary>
        /// Creates the counters.
        /// </summary>
        private static void CreateCounters()
        {
            try
            {
                // Create the counters.
                NumberOfAPICalls = new PerformanceCounter("Sepura", "NumberOfAPICalls", false);
                NumberOfAPICalls.RawValue = 0;

                NumberOfGetAPICalls = new PerformanceCounter("Sepura", "NumberOfGetAPICalls", false);
                NumberOfGetAPICalls.RawValue = 0;

                NumberOfPostAPICalls = new PerformanceCounter("Sepura", "NumberOfPostAPICalls", false);
                NumberOfPostAPICalls.RawValue = 0;

                NumberOfPatchAPICalls = new PerformanceCounter("Sepura", "NumberOfPatchAPICalls", false);
                NumberOfPatchAPICalls.RawValue = 0;

                NumberOfDeleteAPICalls = new PerformanceCounter("Sepura", "NumberOfDeleteAPICalls", false);
                NumberOfDeleteAPICalls.RawValue = 0;

                NumberOfLoginAPICalls = new PerformanceCounter("Sepura", "NumberOfLoginAPICalls", false);
                NumberOfLoginAPICalls.RawValue = 0;

                NumberOfLogoutAPICalls = new PerformanceCounter("Sepura", "NumberOfLogoutAPICalls", false);
                NumberOfLogoutAPICalls.RawValue = 0;

                NumberOfServiceErrors = new PerformanceCounter("Sepura", "NumberOfServiceErrors", false);
                NumberOfServiceErrors.RawValue = 0;

                NumberOfControllerErrors = new PerformanceCounter("Sepura", "NumberOfControllerErrors", false);
                NumberOfControllerErrors.RawValue = 0;
            }
            catch (Exception)
            {
                // Log Exception
            }
        }
        #endregion
    }
}