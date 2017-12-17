using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Configuration.Install;
using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// this class provides static methods to publish messages to WMI
    /// </summary>
    public class InstrumentationProvider
    {
        #region Constructors
        /// <summary>
        /// private constructor so no instance of the class can be created
        /// </summary>
        private InstrumentationProvider()
        {
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// publishes a message to the WMI repository
        /// </summary>
        /// <param name="MessageText">the message text</param>
        /// <param name="Type">the message type</param>
        /// <returns></returns>
        public static InfoMessage PublishMessage(string MessageText, MessageType Type)
        {
            // create a new message
            InfoMessage Message = new InfoMessage();

            // set its properties
            Message.Message = MessageText;
            Message.Guid = Guid.NewGuid().ToString();
            Message.Type = (int)Type;

            try
            {
                // publishes the message to the WMI repository
                Instrumentation.Publish(Message);
            }
            catch (ManagementException)
            {
                return InfoMessage.EmptyMessage();
            }

            // return the message
            return Message;
        }

        /// <summary>
        /// revoke a previously published message from the WMI repository
        /// </summary>
        /// <param name="Message">the message to revoke</param>
        public static void RevokeMessage(InfoMessage Message)
        {
            Instrumentation.Revoke(Message);
        }

        /// <summary>
        /// fires a WMI event
        /// </summary>
        /// <param name="Message">the event message</param>
        /// <param name="Type">the event type</param>
        /// <returns>
        /// has event been fired successfully
        /// </returns>
        public static bool FireEvent(string Message, EventType Type)
        {
            // create a new event
            EventDetails Details = new EventDetails();

            // set the event details
            Details.Message = Message;
            Details.Guid = Guid.NewGuid().ToString();
            Details.Type = (int)Type;

            try
            {
                // fire the event so consumers can consume it
                Details.Fire();
            }
            catch (ManagementException)
            {
                return false;
            }

            // we successfully fired the event
            return true;
        }
        #endregion
    }
}
