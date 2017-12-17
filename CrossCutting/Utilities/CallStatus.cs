using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Contains the context of an external call, typically one between a host and an Sepura.ApplicationServer service.
    /// </summary>
    [DataContract(IsReference=true, Namespace="http://www.sepura.co.uk/IA")]
    public class CallStatus
    {
        public CallStatus() 
        {
        }
        
        /// <summary>
        /// The result of the call. 
        /// </summary>
        public enum enumCallResult
        {
            Success,
            Fault,
            AuthenticationFailure
        }

        /// <summary>
        /// The status of the call
        /// </summary>
        [DataMember]
        public enumCallResult Result = enumCallResult.Success;

        /// <summary>
        /// The validation or other messages to be passed to the caller.
        /// </summary>
        [DataMember]
        public ValidationErrorCollection Messages = new ValidationErrorCollection();

        /// <summary>
        /// The business operation being carried out. Null by default.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BusinessOperation Operation { get; set; }
        
        [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
        public class BusinessOperation
        {
            public BusinessOperation()
            {
                this.CallID = Guid.NewGuid();
            }

            /// <summary>
            /// The ID of the business operation - this is set at configuration time when specifying the Service 
            /// </summary>
            [DataMember]
            public Guid ID { get; set; }
            
            /// <summary>
            /// A Guid assigned to this business operation when an instance of it is created - this can be used to uniquely identify this specific call throughout
            /// the architecture.
            /// </summary>
            [DataMember]
            public Guid CallID { get; set; }

            /// <summary>
            /// The ID of the business operation - this is set at configuration time when specifying the Service 
            /// </summary>
            public string IDString { set { this.ID = new Guid(value); } }
            /// <summary>
            /// The business operation description (such as "Taking the Register", "Saving a Pupils Details", "Adding a SEN Review" etc.
            /// </summary>
            [DataMember]
            public string Description { get; set; }

            /// <summary>
            /// The unique login name of the originating user. This can be null for calls such as AuthenticateUser which can be placed prior to the users
            /// identity being verified. 
            /// </summary>
            [DataMember(EmitDefaultValue=false)]
            public string OriginatingUser { get; set; } 

        }


        public void AddJourneyCommentary(string operation, object value)
        {
            if (this.Journey == null) this.Journey = new List<JourneyItem>();
            this.Journey.Add(new JourneyItem { Operation = operation, Value = value });
        }

        /// <summary>
        /// An item recording the journey of the call between architectural tiers. Used when correllating tracing and audit trail, as well as debugging and 
        /// fault fixing.
        /// </summary>
        [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
        public class JourneyItem 
        {
            /// <summary>
            /// The operation being carried out on the journey
            /// </summary>
            [DataMember]
            public string Operation { get; set; }
            /// <summary>
            /// The value recorded againts the operation (can be null)
            /// </summary>
            [DataMember(EmitDefaultValue=false)]
            public object Value {get;set;}
        }

        /// <summary>
        /// The journey this call status has been on. Null by default.
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        public List<JourneyItem> Journey = null;

    }
}
