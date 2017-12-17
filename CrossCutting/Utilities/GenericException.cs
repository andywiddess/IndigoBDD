using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    [Serializable]
    [KnownType(typeof(Dictionary<string, string>))]
    public class GenericException : FaultException
    {
#pragma warning disable 0649 // Data not initialized to prevent excessive serialization
        private Dictionary<string, string> data;
#if !SILVERLIGHT
        private string stackTrace;
        private string message;
#endif
#pragma warning restore 0649// Data not initialized to prevent excessive serialization


#if !SILVERLIGHT
        public GenericException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            //Overriding this method because of Stack Trace and Data
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            this.message = info.GetString("Message");
            this.data = (Dictionary<string, string>)info.GetValue("Data", typeof(Dictionary<string, string>));
            //this.InnerException = (Exception)info.GetValue("InnerException", typeof(Exception));
            this.HelpLink = info.GetString("HelpURL");
            this.stackTrace = info.GetString("StackTraceString");
            this.HResult = info.GetInt32("HResult");
            this.Source = info.GetString("Source");
            //FaultCodeData[] codeNodes = (FaultCodeData[])info.GetValue("code", typeof(FaultCodeData[]));
            //this.Code = FaultCodeData.Construct(codeNodes);
            //FaultReasonData[] reasonNodes = (FaultReasonData[])info.GetValue("reason", typeof(FaultReasonData[]));
            //this.Reason = FaultReasonData.Construct(reasonNodes);
            //this.Action = info.GetString("action");
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            //Overriding this method because of Stack Trace and Data
            //base.GetObjectData(info, context);
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ClassName", string.Empty, typeof(string));
            info.AddValue("Message", this.message, typeof(string));
            info.AddValue("Data", this.data, typeof(Dictionary<string, string>));
            info.AddValue("InnerException", this.InnerException, typeof(Exception));
            info.AddValue("HelpURL", this.HelpLink, typeof(string));
            info.AddValue("StackTraceString", this.stackTrace, typeof(string));
            info.AddValue("RemoteStackTraceString", string.Empty, typeof(string));
            info.AddValue("RemoteStackIndex", 0, typeof(int));
            info.AddValue("ExceptionMethod", string.Empty, typeof(string));
            info.AddValue("HResult", this.HResult);
            info.AddValue("Source", this.Source, typeof(string));

            //info.AddValue("code", FaultCodeData.GetObjectData(this.Code));
            //info.AddValue("reason", FaultReasonData.GetObjectData(this.Reason));
            //info.AddValue("messageFault", null);
            //info.AddValue("action", this.Action);

        }

        public GenericException(Exception ex) : base(ex.Message)
        {
            SetErrorData(ex.Data);
            this.message = ex.Message;
            this.stackTrace = ex.StackTrace;
            this.HelpLink = ex.HelpLink;
            this.Source = ex.Source;
        }

        public GenericException(string message, System.Collections.IDictionary data) : base(message)
        {
            SetErrorData(data);
            this.message = message;
        }

        /// <summary>
        /// Transfers the data elements for the passed dictionary to the current exception 
        /// </summary>
        /// <param name="data"></param>
        private void SetErrorData(System.Collections.IDictionary data)
        {
            if (data != null && data.Count > 0)
            { 
                this.data = new Dictionary<string,string>();
                foreach (System.Collections.DictionaryEntry dataEntry in data)
                {
                    this.AddData(dataEntry.Key.ToString(), dataEntry.Value.ToString());
                }
            }
        }

        public GenericException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        /// <returns>An object that implements the <see cref="T:System.Collections.IDictionary" /> interface and contains a collection of user-defined key/value pairs. The default is an empty collection.</returns>
        public override System.Collections.IDictionary Data
        {
            get
            {
                if (data == null)
                {
                    data = new System.Collections.Generic.Dictionary<string, string>();
                }
                return data;
            }
        }

        public override string StackTrace
        {
            get
            {
                return this.stackTrace;
            }
        }

        public override string Message
        {
            get
            {
                return this.message;
            }
        }
        #endif

        /// <summary>
        /// Adds the key/value element to the Data Dictionary of the exception. 
        /// If the key already exists then the value against that item is reset
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData(string key, string value)
        {
            if (this.data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

    }
}
