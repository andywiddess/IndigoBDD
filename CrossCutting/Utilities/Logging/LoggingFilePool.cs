using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// A class for managing a pool of log files to prevent excessive opening a closing.
    /// </summary>
    public static class LoggingFilePool
    {

        private static TDictionary<string, System.IO.Stream> bufferedStreams = new TDictionary<string, System.IO.Stream>();

        private static object lockObject = new object();

        /// <summary>
        /// Get or create/open a buffered output stream for writing to
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static System.IO.Stream GetBufferedOutputStream(string fileName)
        {
            while (flushing)
            {
                System.Threading.Thread.Sleep(50);
            }


            if (bufferedStreams.ContainsKey(fileName))
            {
                return bufferedStreams[fileName];
            }


            System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.Read);
            lock (lockObject)
            {
                ensureFolderExists(System.IO.Path.GetDirectoryName(fileName));

                if (!bufferedStreams.AddIfNotExists(fileName, fileStream))
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }

                return bufferedStreams[fileName];
            }




        }

        private static void ensureFolderExists(string telemetryLogFolder)
        {
            if (telemetryLogFolder == null)
            {
                return;
            }

            if (!System.IO.Directory.Exists(telemetryLogFolder))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(telemetryLogFolder);

                }
                catch (Exception e)
                {
                    e.Data.Add("Directory", telemetryLogFolder);
                    throw;
                }
            }
        }

        private volatile static bool flushing = false;

        /// <summary>
        /// Flush and close all output files. This is non-destructive and can be called many times.
        /// </summary>
        public static void Flush()
        {
            flushing = true;
            lock (lockObject)
            {
                foreach (System.IO.Stream stream in bufferedStreams.Values)
                {
                    stream.Flush();
                    stream.Close();
                    stream.Dispose();
                }
                bufferedStreams.Clear();
            }
            flushing = false;
        }
    }
}
