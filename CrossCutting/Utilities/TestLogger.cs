using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Very lightweight test recording facility.
    /// </summary>
    public static class TestLogger
    {
        private static int testCount = 0;
        public static void Test(bool success, string message)
        {
            // For the time being just emit an Assert. Later on we will want to log this.
            System.Diagnostics.Debug.Assert(success, message);
            testCount ++;
        }

        public static void Test(bool success)
        {
            Test(success, "");
        }

        public static int GetTestCount
        {
            get { return testCount;}
        }
    }
}
