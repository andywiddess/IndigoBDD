using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Stream utility
    /// </summary>
    public class Streamutils
    {
        /// <summary>
        /// Return a raw stream for the json string. http://stackoverflow.com/questions/3078397/returning-raw-json-string-in-wcf
        /// </summary>
        /// <param name="jsonresponse"></param>
        /// <returns></returns>
        public static MemoryStream PackToRawJsonMemoryStream(string jsonresponse)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Cache-Control", "no-cache");
                return new MemoryStream(Encoding.UTF8.GetBytes(jsonresponse));
            }
            return null;
        }

    }
}
