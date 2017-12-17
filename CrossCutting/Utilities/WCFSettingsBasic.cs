using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Text;

namespace Sepura.ApplicationServer.Services
{
    /// <summary>
    /// Standard hard coded set of settings for WCF
    /// 
    /// In this scenario we are using binary encoding instead of WsHTTPBinding. See the link below for comparison.
    /// http://www.zamd.net/2008/06/18/SOAPMessageSizeOptimizationEncodingVsCompression.aspx
    /// </summary>
    public static class WCFSettingsBasic
    {
        public static Binding ServerBinding(string url)
        {
            if (url.ToLower().StartsWith("net.tcp"))
            {
                return netTcpBinding(url);
            }
            else
            {
                return httpBinding(url);
            }
        }

        public static Binding httpBinding(string url)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            if (url.ToUpper().StartsWith("HTTPS"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                
            }
            else
            {

            }

            #if !SILVERLIGHT

            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;

            #endif

            return binding;
        }



        private static Binding netTcpBinding(string url)
        {
            #if !SILVERLIGHT

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            
            binding.TransactionFlow = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

            binding.ReliableSession.Enabled = false;
            binding.ReliableSession.Ordered = true;
            binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);

            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;

            return binding;

            #else

            throw new NotSupportedException("TCP Binding Is not supported in Silverlight");

            #endif

            
        }


        public static Binding SilverlightSecurityServerBinding(string url, bool anon)
        {

            BasicHttpBindingWithSoapEncoding binding = new BasicHttpBindingWithSoapEncoding();
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            if (url.ToUpper().StartsWith("HTTPS"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
            }
            else
            {
                if (!anon)
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            }
            return binding;
        }
    }

    public static class ProviderFactory
    {
        private static IRemoteCallDataPacketSniffer dataPacketSniffer = null;

        /// <summary>
        /// Register the remote call data packet sniffer
        /// </summary>
        /// <param name="sniffer">The sniffer.</param>
        public static void RegisterDataPacketSniffer(IRemoteCallDataPacketSniffer sniffer)
        {
            dataPacketSniffer = sniffer;
        }

        /// <summary>
        /// Get the current registered remote call data packet sniffer
        /// </summary>
        /// <returns></returns>
        public static IRemoteCallDataPacketSniffer GetCurrentDataPacketSniffer()
        {
            return dataPacketSniffer;
        }
    }

    public interface IRemoteCallDataPacketSniffer
    {
        /// <summary>
        /// Notify about Data Packate Transfer
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="bytes"></param>
        void NotifyTransfer(enumDataPacketMode mode, long bytes);

        /// <summary>
        /// Reset the counters to 0
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// Data Packet Mode
    /// </summary>
    public enum enumDataPacketMode
    {
        /// <summary>
        /// Sent
        /// </summary>
        SENT = 0,
        /// <summary>
        /// Received
        /// </summary>
        RECEIVED = 1
    }

    #region Basic Http Binding With Soap Encoding
    public class BasicHttpBindingWithSoapEncoding : Binding
    {
        // Fields
        private HttpsTransportBindingElement httpsTransport;
        private HttpTransportBindingElement httpTransport;
        private BasicHttpSecurity security;
        private SoapMessageEncodingBindingElement soapEncoding;

        // Methods
        public BasicHttpBindingWithSoapEncoding()
            : this(BasicHttpSecurityMode.None)
        {
        }

        public BasicHttpBindingWithSoapEncoding(BasicHttpSecurityMode securityMode)
        {
            this.security = new BasicHttpSecurity();
            this.Initialize();
            this.security.Mode = securityMode;
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = new BindingElementCollection();
            SecurityBindingElement item = this.CreateMessageSecurity();
            if (item != null)
            {
                elements.Add(item);
            }
            elements.Add(this.soapEncoding);
            elements.Add(this.GetTransport());
            return elements.Clone();
        }

        private SecurityBindingElement CreateMessageSecurity()
        {
            return this.security.CreateMessageSecurity();
        }

        private TransportBindingElement GetTransport()
        {
            if ((this.security.Mode != BasicHttpSecurityMode.Transport) && (this.security.Mode != BasicHttpSecurityMode.TransportWithMessageCredential))
            {
                return this.httpTransport;
            }
            return this.httpsTransport;
        }

        private void Initialize()
        {
            this.httpTransport = new HttpTransportBindingElement();
            this.httpsTransport = new HttpsTransportBindingElement();
            this.soapEncoding = new SoapMessageEncodingBindingElement(MessageVersion.Soap11, new System.Text.UTF8Encoding(false));
        }

        // Properties
        public EnvelopeVersion EnvelopeVersion
        {
            get
            {
                return EnvelopeVersion.Soap11;
            }
        }

        public int MaxBufferSize
        {
            get
            {
                return this.httpTransport.MaxBufferSize;
            }
            set
            {
                this.httpTransport.MaxBufferSize = value;
                this.httpsTransport.MaxBufferSize = value;
            }
        }

        public long MaxReceivedMessageSize
        {
            get
            {
                return this.httpTransport.MaxReceivedMessageSize;
            }
            set
            {
                this.httpTransport.MaxReceivedMessageSize = value;
                this.httpsTransport.MaxReceivedMessageSize = value;
            }
        }

        public override string Scheme
        {
            get
            {
                return this.GetTransport().Scheme;
            }
        }

        public BasicHttpSecurity Security
        {
            get
            {
                return this.security;
            }
        }

        public System.Text.Encoding SoapEncoding
        {
            get
            {
                return this.soapEncoding.WriteEncoding;
            }
            set
            {
                this.soapEncoding.WriteEncoding = value;
            }
        }

        public TransferMode TransferMode
        {
            get
            {
                return this.httpTransport.TransferMode;
            }
            set
            {
                this.httpTransport.TransferMode = value;
                this.httpsTransport.TransferMode = value;
            }
        }
    }

    public sealed class BasicHttpSecurity
    {
        // Fields
        internal const BasicHttpSecurityMode DefaultMode = BasicHttpSecurityMode.None;
        private BasicHttpSecurityMode mode;

        // Methods
        internal BasicHttpSecurity()
            : this(BasicHttpSecurityMode.None)
        {
        }

        private BasicHttpSecurity(BasicHttpSecurityMode mode)
        {
            this.Mode = mode;
        }

        internal SecurityBindingElement CreateMessageSecurity()
        {
            if (this.mode == BasicHttpSecurityMode.TransportWithMessageCredential)
            {
                return SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            }
            return null;
        }

        // Properties
        public BasicHttpSecurityMode Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }
    }

    public class SoapMessageEncodingBindingElement : MessageEncodingBindingElement
    {
        // Fields
        private MessageVersion messageVersion;
        private System.Text.Encoding writeEncoding;

        // Methods
        public SoapMessageEncodingBindingElement()
            : this(MessageVersion.Default, new System.Text.UTF8Encoding(false))
        {
        }

        private SoapMessageEncodingBindingElement(SoapMessageEncodingBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            this.writeEncoding = elementToBeCloned.writeEncoding;
            this.messageVersion = elementToBeCloned.messageVersion;
        }

        public SoapMessageEncodingBindingElement(MessageVersion messageVersion, System.Text.Encoding writeEncoding)
        {
            if (messageVersion == null)
            {
                throw new ArgumentNullException("messageVersion");
            }
            if (writeEncoding == null)
            {
                throw new ArgumentNullException("writeEncoding");
            }

            this.messageVersion = messageVersion;
            this.writeEncoding = writeEncoding;
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return base.BuildChannelFactory<TChannel>(context);
        }

        public override BindingElement Clone()
        {
            return new SoapMessageEncodingBindingElement(this);
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new SoapMessageEncoderFactory(this.MessageVersion, this.WriteEncoding);
        }

        // Properties
        public override MessageVersion MessageVersion
        {
            get
            {
                return this.messageVersion;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("MessageVersion");
                }
                this.messageVersion = value;
            }
        }

        public System.Text.Encoding WriteEncoding
        {
            get
            {
                return this.writeEncoding;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("WriteEncoding");
                }

                this.writeEncoding = value;
            }
        }
    }

    internal class SoapMessageEncoderFactory : MessageEncoderFactory
    {
        // Fields
        private SoapMessageEncoder messageEncoder;
        internal const string Soap11MediaType = "text/xml";
        internal const string Soap12MediaType = "application/soap+xml";
        private const string XmlMediaType = "application/xml";

        // Methods
        public SoapMessageEncoderFactory(MessageVersion version, System.Text.Encoding writeEncoding)
        {
            this.messageEncoder = new SoapMessageEncoder(version, writeEncoding);
        }

        internal static string GetContentType(string mediaType, System.Text.Encoding encoding)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}; charset={1}", mediaType, "utf-8");
        }

        internal static string GetMediaType(MessageVersion version)
        {
            if (version.Envelope == EnvelopeVersion.Soap12)
            {
                return "application/soap+xml";
            }
            if (version.Envelope == EnvelopeVersion.Soap11)
            {
                return "text/xml";
            }
            if (version.Envelope != EnvelopeVersion.None)
            {
                throw new InvalidOperationException("Envelope Version Not Supported ");
            }
            return "application/xml";
        }

        // Properties
        public override MessageEncoder Encoder
        {
            get
            {
                return this.messageEncoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.messageEncoder.MessageVersion;
            }
        }

        // Nested Types
        private class SoapMessageEncoder : MessageEncoder
        {
            // Fields
            private string contentType;
            private string mediaType;
            private MessageVersion version;
            private System.Text.Encoding writeEncoding;
            private IRemoteCallDataPacketSniffer dataPacketSniffer;

            // Methods
            public SoapMessageEncoder(MessageVersion version, System.Text.Encoding writeEncoding)
            {
                if (version == null)
                {
                    throw new ArgumentNullException("version");
                }
                if (writeEncoding == null)
                {
                    throw new ArgumentNullException("writeEncoding");
                }

                this.writeEncoding = writeEncoding;
                this.version = version;
                this.mediaType = SoapMessageEncoderFactory.GetMediaType(version);
                this.contentType = SoapMessageEncoderFactory.GetContentType(this.mediaType, writeEncoding);
                if (((version.Envelope != EnvelopeVersion.Soap12) && (version.Envelope != EnvelopeVersion.Soap11)) && (version.Envelope != EnvelopeVersion.None))
                {
                    throw new InvalidOperationException("Envelope Version Not Supported");
                }
                this.dataPacketSniffer = ProviderFactory.GetCurrentDataPacketSniffer();
            }

            private System.Xml.XmlDictionaryWriter CreateWriter(System.IO.Stream stream)
            {
                return System.Xml.XmlDictionaryWriter.CreateTextWriter(stream, this.writeEncoding, false);
            }

            public override bool IsContentTypeSupported(string contentType)
            {
                if (contentType == null)
                {
                    throw new ArgumentNullException("contentType");
                }
                if (base.IsContentTypeSupported(contentType))
                {
                    return true;
                }

                return false;
            }

            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                byte[] msgContents = new byte[buffer.Count];
                Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
                bufferManager.ReturnBuffer(buffer.Array);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(msgContents);

                if (dataPacketSniffer != null)
                {
                    dataPacketSniffer.NotifyTransfer(enumDataPacketMode.RECEIVED, msgContents.Length);
                }
                return ReadMessage(stream, int.MaxValue);

            }

            public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
            {
                System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stream);
                Message message = Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
                return message;
            }

            public override void WriteMessage(Message message, System.IO.Stream stream)
            {
                System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stream);
                message.WriteMessage(writer);
                writer.Close();
            }

            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
            {
                byte[] messageBytes = new byte[0];
                int messageLength = 0;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stream))
                    {
                        message.WriteMessage(writer);
                        writer.Close();
                    }
                    messageBytes = stream.GetBuffer();
                    messageLength = (int)stream.Position;
                    stream.Close();
                }

                int totalLength = messageLength + messageOffset;
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                if (dataPacketSniffer != null)
                {
                    dataPacketSniffer.NotifyTransfer(enumDataPacketMode.SENT, totalLength);
                }
                return byteArray;
            }

            // Properties
            public override string ContentType
            {
                get
                {
                    return this.contentType;
                }
            }

            public override string MediaType
            {
                get
                {
                    return this.mediaType;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return this.version;
                }
            }
        }
    }
    #endregion
    
    #region Net Tcp Binding With Binary Encoding
    public class NetTcpBindingWithBinaryEncoding : Binding
    {
        // Fields
        private BinaryMessageEncodingBindingElement messageEncoding;
        private TcpTransportBindingElement tcpTransport;

        // Methods
        public NetTcpBindingWithBinaryEncoding()
        {
            this.messageEncoding = new BinaryMessageEncodingBindingElement();
            this.messageEncoding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            this.messageEncoding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            this.messageEncoding.ReaderQuotas.MaxDepth = int.MaxValue;
            this.messageEncoding.ReaderQuotas.MaxNameTableCharCount = 16384;
            this.messageEncoding.ReaderQuotas.MaxBytesPerRead = 4096;
            this.tcpTransport = new TcpTransportBindingElement();
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = new BindingElementCollection();
            elements.Add(this.messageEncoding);
            elements.Add(this.tcpTransport);
            return elements.Clone();
        }

        public int MaxBufferSize
        {
            get
            {
                return this.tcpTransport.MaxBufferSize;
            }
            set
            {
                this.tcpTransport.MaxBufferSize = value;
            }
        }

        public long MaxReceivedMessageSize
        {
            get
            {
                return this.tcpTransport.MaxReceivedMessageSize;
            }
            set
            {
                this.tcpTransport.MaxReceivedMessageSize = value;
            }
        }

        public override string Scheme
        {
            get { return this.tcpTransport.Scheme; }
        }
    }

    public class BinaryMessageEncodingBindingElement : MessageEncodingBindingElement
    {
        // Fields
        private MessageVersion messageVersion;

        // Methods
        public BinaryMessageEncodingBindingElement()
        {
            this.messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12);
        }

        private BinaryMessageEncodingBindingElement(BinaryMessageEncodingBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            this.messageVersion = elementToBeCloned.messageVersion;
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return base.BuildChannelFactory<TChannel>(context);
        }

        public override BindingElement Clone()
        {
            return new BinaryMessageEncodingBindingElement(this);
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new BinaryMessageEncoderFactory(this.messageVersion);
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.messageVersion;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("MessageVersion");
                }
                this.messageVersion = value;
            }
        }

        private XmlDictionaryReaderQuotas readerQuotas;

        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                if (this.readerQuotas == null)
                {
                    this.readerQuotas = new XmlDictionaryReaderQuotas();
                }
                return this.readerQuotas;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                value.CopyTo(this.readerQuotas);
            }
        }
    }

    internal class BinaryMessageEncoderFactory : MessageEncoderFactory
    {
        // Fields
        private MessageVersion messageVersion;
        private BinaryMessageEncoder messageEncoder;

        // Methods
        public BinaryMessageEncoderFactory(MessageVersion version)
        {
            this.messageVersion = version;
            this.messageEncoder = new BinaryMessageEncoder(this);
        }

        // Properties
        public override MessageEncoder Encoder
        {
            get
            {
                return this.messageEncoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.messageEncoder.MessageVersion;
            }
        }

        public string ContentType
        {
            get
            {
                return "application/soap+msbinsession1";
            }
        }

        public IXmlDictionary Dictionary
        {
            get
            {
                return ServiceModelDictionary.Version1;
            }
        }

        public override MessageEncoder CreateSessionEncoder()
        {
            return base.CreateSessionEncoder();
        }

        // Nested Types
        private class BinaryMessageEncoder : MessageEncoder
        {
            // Fields
            private BinaryMessageEncoderFactory factory;
            private IRemoteCallDataPacketSniffer dataPacketSniffer;
            private XmlBinaryReaderSession readerSession;
            private BinaryMessageEncoderFactory.XmlBinaryWriterSessionWithQuota writerSession;
            private int idCounter;
            private bool isReaderSessionInvalid;

            // Methods
            public BinaryMessageEncoder(BinaryMessageEncoderFactory factory)
            {
                this.factory = factory;
                this.dataPacketSniffer = ProviderFactory.GetCurrentDataPacketSniffer();
            }

            public override bool IsContentTypeSupported(string contentType)
            {
                //return base.IsContentTypeSupported(contentType);
                return true;
            }

            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                if (this.readerSession == null)
                {
                    this.readerSession = new XmlBinaryReaderSession();
                    //this.messagePatterns = new MessagePatterns(this.factory.Dictionary, this.readerSession, this.MessageVersion);
                }
                buffer = this.ExtractSessionInformationFromMessage(buffer);

                byte[] msgContents = new byte[buffer.Count];
                Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
                bufferManager.ReturnBuffer(buffer.Array);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(msgContents);

                if (dataPacketSniffer != null)
                {
                    dataPacketSniffer.NotifyTransfer(enumDataPacketMode.RECEIVED, msgContents.Length);
                }
                return ReadMessage(stream, int.MaxValue, contentType);
            }

            public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
            {
                System.Xml.XmlDictionaryReader reader = System.Xml.XmlDictionaryReader.CreateBinaryReader(stream, this.factory.Dictionary, XmlDictionaryReaderQuotas.Max, this.readerSession);
                Message message = Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
                return message;
            }

            public override void WriteMessage(Message message, System.IO.Stream stream)
            {
                System.Xml.XmlDictionaryWriter writer = System.Xml.XmlDictionaryWriter.CreateBinaryWriter(stream);
                message.WriteMessage(writer);
                writer.Close();
            }

            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
            {
                message.Properties.Encoder = this;
                if (this.writerSession == null)
                {
                    this.writerSession = new XmlBinaryWriterSessionWithQuota();
                }
                byte[] messageBytes = new byte[0];
                int messageLength = 0;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    //session = XMLBinaryWriterSessionWithQuota
                    using (System.Xml.XmlDictionaryWriter writer = System.Xml.XmlDictionaryWriter.CreateBinaryWriter(stream, dictionary: this.factory.Dictionary, session: this.writerSession, ownsStream: false))
                    {
                        message.WriteMessage(writer);
                        writer.Close();
                    }
                    messageBytes = stream.GetBuffer();
                    messageLength = (int)stream.Position;
                    stream.Close();
                }

                int totalLength = messageLength + messageOffset;
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                ArraySegment<byte> byteArrayBeforSessionInfo = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                ArraySegment<byte> byteArray = AddSessionInformationToMessage(byteArrayBeforSessionInfo, bufferManager, maxMessageSize);
                if (dataPacketSniffer != null)
                {
                    dataPacketSniffer.NotifyTransfer(enumDataPacketMode.SENT, totalLength);
                }
                return byteArray;
            }

            private ArraySegment<byte> ExtractSessionInformationFromMessage(ArraySegment<byte> messageData)
            {
                int num3;
                int num4;
                if (this.isReaderSessionInvalid)
                {
                    throw new Exception("BinaryEncoderSessionInvalid");
                }
                byte[] array = messageData.Array;
                bool flag = true;
                try
                {
                    IntDecoder decoder = new IntDecoder();
                    int num2 = decoder.Decode(array, messageData.Offset, messageData.Count);
                    int num = decoder.Value;
                    if (num > messageData.Count)
                    {
                        throw new Exception("BinaryEncoderSessionMalformed");
                    }
                    num3 = (messageData.Offset + num2) + num;
                    num4 = (messageData.Count - num2) - num;
                    if (num4 < 0)
                    {
                        throw new Exception("BinaryEncoderSessionMalformed");
                    }
                    if (num > 0)
                    {
                        int size = num;
                        int offset = messageData.Offset + num2;
                        while (size > 0)
                        {
                            decoder.Reset();
                            int num7 = decoder.Decode(array, offset, size);
                            int count = decoder.Value;
                            offset += num7;
                            size -= num7;
                            if (count > size)
                            {
                                throw new Exception("BinaryEncoderSessionMalformed");
                            }
                            string str = Encoding.UTF8.GetString(array, offset, count);
                            offset += count;
                            size -= count;
                            this.readerSession.Add(this.idCounter, str);
                            this.idCounter++;
                        }
                    }
                    flag = false;
                }
                finally
                {
                    if (flag)
                    {
                        this.isReaderSessionInvalid = true;
                    }
                }
                return new ArraySegment<byte>(array, num3, num4);
            }

            private ArraySegment<byte> AddSessionInformationToMessage(ArraySegment<byte> messageData, BufferManager bufferManager, int maxMessageSize)
            {
                int num = 0;
                byte[] array = messageData.Array;
                if (this.writerSession.HasNewStrings)
                {
                    IList<XmlDictionaryString> newStrings = this.writerSession.GetNewStrings();
                    for (int i = 0; i < newStrings.Count; i++)
                    {
                        int byteCount = Encoding.UTF8.GetByteCount(newStrings[i].Value);
                        num += IntEncoder.GetEncodedSize(byteCount) + byteCount;
                    }
                    int num4 = messageData.Offset + messageData.Count;
                    int num5 = maxMessageSize - num4;
                    if ((num5 - num) < 0)
                    {
                        throw new QuotaExceededException(string.Format("Max Sent Message Size Exceeded {0}", maxMessageSize));
                    }
                    int bufferSize = (messageData.Offset + messageData.Count) + num;
                    if (array.Length < bufferSize)
                    {
                        byte[] dst = bufferManager.TakeBuffer(bufferSize);
                        Buffer.BlockCopy(array, messageData.Offset, dst, messageData.Offset, messageData.Count);
                        bufferManager.ReturnBuffer(array);
                        array = dst;
                    }
                    Buffer.BlockCopy(array, messageData.Offset, array, messageData.Offset + num, messageData.Count);
                    int num7 = messageData.Offset;
                    for (int j = 0; j < newStrings.Count; j++)
                    {
                        string s = newStrings[j].Value;
                        int num9 = Encoding.UTF8.GetByteCount(s);
                        num7 += IntEncoder.Encode(num9, array, num7);
                        num7 += Encoding.UTF8.GetBytes(s, 0, s.Length, array, num7);
                    }
                    this.writerSession.ClearNewStrings();
                }
                int encodedSize = IntEncoder.GetEncodedSize(num);
                int offset = messageData.Offset - encodedSize;
                int count = (encodedSize + messageData.Count) + num;
                IntEncoder.Encode(num, array, offset);
                return new ArraySegment<byte>(array, offset, count);
            }

            // Properties
            public override string ContentType
            {
                get
                {
                    return this.factory.ContentType;
                }
            }

            public override string MediaType
            {
                get
                {
                    return this.factory.ContentType;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return this.factory.messageVersion;
                }
            }


        }

        private class XmlBinaryWriterSessionWithQuota : XmlBinaryWriterSession
        {
            // Fields
            private List<XmlDictionaryString> newStrings;

            // Methods
            public void ClearNewStrings()
            {
                this.newStrings = null;
            }

            public IList<XmlDictionaryString> GetNewStrings()
            {
                return this.newStrings;
            }

            public override bool TryAdd(XmlDictionaryString s, out int key)
            {
                if (!base.TryAdd(s, out key))
                {
                    return false;
                }
                if (this.newStrings == null)
                {
                    this.newStrings = new List<XmlDictionaryString>();
                }
                this.newStrings.Add(s);
                return true;
            }

            // Properties
            public bool HasNewStrings
            {
                get
                {
                    return (this.newStrings != null);
                }
            }
        }
    }

    internal class ServiceModelDictionary : IXmlDictionary
    {
        // Fields
        private int count;
        private Dictionary<string, int> dictionary;
        private XmlDictionaryString[] dictionaryStrings1;
        private XmlDictionaryString[] dictionaryStrings2;
        private ServiceModelStringsVersion1 strings;
        public static readonly ServiceModelDictionary Version1 = new ServiceModelDictionary(new ServiceModelStringsVersion1());
        private XmlDictionaryString[] versionedDictionaryStrings;

        // Methods
        public ServiceModelDictionary(ServiceModelStringsVersion1 strings)
        {
            this.strings = strings;
            this.count = strings.Count;
        }

        public XmlDictionaryString CreateString(string value, int key)
        {
            return new XmlDictionaryString(this, value, key);
        }

        public bool TryLookup(int key, out XmlDictionaryString value)
        {
            XmlDictionaryString str;
            if ((key < 0) || (key >= this.count))
            {
                value = null;
                return false;
            }
            if (key < 0x20)
            {
                if (this.dictionaryStrings1 == null)
                {
                    this.dictionaryStrings1 = new XmlDictionaryString[0x20];
                }
                str = this.dictionaryStrings1[key];
                if (str == null)
                {
                    str = this.CreateString(this.strings[key], key);
                    this.dictionaryStrings1[key] = str;
                }
            }
            else
            {
                if (this.dictionaryStrings2 == null)
                {
                    this.dictionaryStrings2 = new XmlDictionaryString[this.count - 0x20];
                }
                str = this.dictionaryStrings2[key - 0x20];
                if (str == null)
                {
                    str = this.CreateString(this.strings[key], key);
                    this.dictionaryStrings2[key - 0x20] = str;
                }
            }
            value = str;
            return true;
        }

        public bool TryLookup(string key, out XmlDictionaryString value)
        {
            int num2;
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (this.dictionary == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(this.count);
                for (int i = 0; i < this.count; i++)
                {
                    dictionary.Add(this.strings[i], i);
                }
                this.dictionary = dictionary;
            }
            if (this.dictionary.TryGetValue(key, out num2))
            {
                return this.TryLookup(num2, out value);
            }
            value = null;
            return false;
        }

        public bool TryLookup(XmlDictionaryString key, out XmlDictionaryString value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (key.Dictionary == this)
            {
                value = key;
                return true;
            }
            if (key.Dictionary == CurrentVersion)
            {
                if (this.versionedDictionaryStrings == null)
                {
                    this.versionedDictionaryStrings = new XmlDictionaryString[CurrentVersion.count];
                }
                XmlDictionaryString str = this.versionedDictionaryStrings[key.Key];
                if (str == null)
                {
                    if (!this.TryLookup(key.Value, out str))
                    {
                        value = null;
                        return false;
                    }
                    this.versionedDictionaryStrings[key.Key] = str;
                }
                value = str;
                return true;
            }
            value = null;
            return false;
        }

        // Properties
        public static ServiceModelDictionary CurrentVersion
        {
            get
            {
                return Version1;
            }
        }
    }

    internal class ServiceModelStringsVersion1
    {
        // Fields
        public static readonly string[] Strings = new string[] { 
        "mustUnderstand", "Envelope", "http://www.w3.org/2003/05/soap-envelope", "http://www.w3.org/2005/08/addressing", "Header", "Action", "To", "Body", "Algorithm", "RelatesTo", "http://www.w3.org/2005/08/addressing/anonymous", "URI", "Reference", "MessageID", "Id", "Identifier", 
        "http://schemas.xmlsoap.org/ws/2005/02/rm", "Transforms", "Transform", "DigestMethod", "DigestValue", "Address", "ReplyTo", "SequenceAcknowledgement", "AcknowledgementRange", "Upper", "Lower", "BufferRemaining", "http://schemas.microsoft.com/ws/2006/05/rm", "http://schemas.xmlsoap.org/ws/2005/02/rm/SequenceAcknowledgement", "SecurityTokenReference", "Sequence", 
        "MessageNumber", "http://www.w3.org/2000/09/xmldsig#", "http://www.w3.org/2000/09/xmldsig#enveloped-signature", "KeyInfo", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", "http://www.w3.org/2001/04/xmlenc#", "http://schemas.xmlsoap.org/ws/2005/02/sc", "DerivedKeyToken", "Nonce", "Signature", "SignedInfo", "CanonicalizationMethod", "SignatureMethod", "SignatureValue", "DataReference", "EncryptedData", 
        "EncryptionMethod", "CipherData", "CipherValue", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", "Security", "Timestamp", "Created", "Expires", "Length", "ReferenceList", "ValueType", "Type", "EncryptedHeader", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd", "RequestSecurityTokenResponseCollection", "http://schemas.xmlsoap.org/ws/2005/02/trust", 
        "http://schemas.xmlsoap.org/ws/2005/02/trust#BinarySecret", "http://schemas.microsoft.com/ws/2006/02/transactions", "s", "Fault", "MustUnderstand", "role", "relay", "Code", "Reason", "Text", "Node", "Role", "Detail", "Value", "Subcode", "NotUnderstood", 
        "qname", "", "From", "FaultTo", "EndpointReference", "PortType", "ServiceName", "PortName", "ReferenceProperties", "RelationshipType", "Reply", "a", "http://schemas.xmlsoap.org/ws/2006/02/addressingidentity", "Identity", "Spn", "Upn", 
        "Rsa", "Dns", "X509v3Certificate", "http://www.w3.org/2005/08/addressing/fault", "ReferenceParameters", "IsReferenceParameter", "http://www.w3.org/2005/08/addressing/reply", "http://www.w3.org/2005/08/addressing/none", "Metadata", "http://schemas.xmlsoap.org/ws/2004/08/addressing", "http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous", "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault", "http://schemas.xmlsoap.org/ws/2004/06/addressingex", "RedirectTo", "Via", "http://www.w3.org/2001/10/xml-exc-c14n#", 
        "PrefixList", "InclusiveNamespaces", "ec", "SecurityContextToken", "Generation", "Label", "Offset", "Properties", "Cookie", "wsc", "http://schemas.xmlsoap.org/ws/2004/04/sc", "http://schemas.xmlsoap.org/ws/2004/04/security/sc/dk", "http://schemas.xmlsoap.org/ws/2004/04/security/sc/sct", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/RST/SCT", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/RSTR/SCT", "RenewNeeded", 
        "BadContextToken", "c", "http://schemas.xmlsoap.org/ws/2005/02/sc/dk", "http://schemas.xmlsoap.org/ws/2005/02/sc/sct", "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT", "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT/Cancel", "http://www.w3.org/2001/04/xmlenc#aes128-cbc", "http://www.w3.org/2001/04/xmlenc#kw-aes128", "http://www.w3.org/2001/04/xmlenc#aes192-cbc", "http://www.w3.org/2001/04/xmlenc#kw-aes192", "http://www.w3.org/2001/04/xmlenc#aes256-cbc", "http://www.w3.org/2001/04/xmlenc#kw-aes256", 
        "http://www.w3.org/2001/04/xmlenc#des-cbc", "http://www.w3.org/2000/09/xmldsig#dsa-sha1", "http://www.w3.org/2001/10/xml-exc-c14n#WithComments", "http://www.w3.org/2000/09/xmldsig#hmac-sha1", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://schemas.xmlsoap.org/ws/2005/02/sc/dk/p_sha1", "http://www.w3.org/2001/04/xmlenc#ripemd160", "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p", "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "http://www.w3.org/2001/04/xmlenc#rsa-1_5", "http://www.w3.org/2000/09/xmldsig#sha1", "http://www.w3.org/2001/04/xmlenc#sha256", "http://www.w3.org/2001/04/xmlenc#sha512", "http://www.w3.org/2001/04/xmlenc#tripledes-cbc", "http://www.w3.org/2001/04/xmlenc#kw-tripledes", 
        "http://schemas.xmlsoap.org/2005/02/trust/tlsnego#TLS_Wrap", "http://schemas.xmlsoap.org/2005/02/trust/spnego#GSS_Wrap", "http://schemas.microsoft.com/ws/2006/05/security", "dnse", "o", "Password", "PasswordText", "Username", "UsernameToken", "BinarySecurityToken", "EncodingType", "KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Text", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509SubjectKeyIdentifier", 
        "http://docs.oasis-open.org/wss/oasis-wss-kerberos-token-profile-1.1#GSS_Kerberosv5_AP_REQ", "http://docs.oasis-open.org/wss/oasis-wss-kerberos-token-profile-1.1#GSS_Kerberosv5_AP_REQ1510", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.0#SAMLAssertionID", "Assertion", "urn:oasis:names:tc:SAML:1.0:assertion", "http://docs.oasis-open.org/wss/oasis-wss-rel-token-profile-1.0.pdf#license", "FailedAuthentication", "InvalidSecurityToken", "InvalidSecurity", "k", "SignatureConfirmation", "TokenType", "http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#ThumbprintSHA1", "http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#EncryptedKey", "http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#EncryptedKeySHA1", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1", 
        "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID", "AUTH-HASH", "RequestSecurityTokenResponse", "KeySize", "RequestedTokenReference", "AppliesTo", "Authenticator", "CombinedHash", "BinaryExchange", "Lifetime", "RequestedSecurityToken", "Entropy", "RequestedProofToken", "ComputedKey", "RequestSecurityToken", 
        "RequestType", "Context", "BinarySecret", "http://schemas.xmlsoap.org/ws/2005/02/trust/spnego", " http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego", "wst", "http://schemas.xmlsoap.org/ws/2004/04/trust", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/Issue", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/CK/PSHA1", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/SymmetricKey", "http://schemas.xmlsoap.org/ws/2004/04/security/trust/Nonce", "KeyType", "http://schemas.xmlsoap.org/ws/2004/04/trust/SymmetricKey", "http://schemas.xmlsoap.org/ws/2004/04/trust/PublicKey", 
        "Claims", "InvalidRequest", "RequestFailed", "SignWith", "EncryptWith", "EncryptionAlgorithm", "CanonicalizationAlgorithm", "ComputedKeyAlgorithm", "UseKey", "http://schemas.microsoft.com/net/2004/07/secext/WS-SPNego", "http://schemas.microsoft.com/net/2004/07/secext/TLSNego", "t", "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/SymmetricKey", 
        "http://schemas.xmlsoap.org/ws/2005/02/trust/CK/PSHA1", "http://schemas.xmlsoap.org/ws/2005/02/trust/Nonce", "RenewTarget", "CancelTarget", "RequestedTokenCancelled", "RequestedAttachedReference", "RequestedUnattachedReference", "IssuedTokens", "http://schemas.xmlsoap.org/ws/2005/02/trust/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/PublicKey", "Access", "AccessDecision", "Advice", "AssertionID", "AssertionIDReference", 
        "Attribute", "AttributeName", "AttributeNamespace", "AttributeStatement", "AttributeValue", "Audience", "AudienceRestrictionCondition", "AuthenticationInstant", "AuthenticationMethod", "AuthenticationStatement", "AuthorityBinding", "AuthorityKind", "AuthorizationDecisionStatement", "Binding", "Condition", "Conditions", 
        "Decision", "DoNotCacheCondition", "Evidence", "IssueInstant", "Issuer", "Location", "MajorVersion", "MinorVersion", "NameIdentifier", "Format", "NameQualifier", "Namespace", "NotBefore", "NotOnOrAfter", "saml", "Statement", 
        "Subject", "SubjectConfirmation", "SubjectConfirmationData", "ConfirmationMethod", "urn:oasis:names:tc:SAML:1.0:cm:holder-of-key", "urn:oasis:names:tc:SAML:1.0:cm:sender-vouches", "SubjectLocality", "DNSAddress", "IPAddress", "SubjectStatement", "urn:oasis:names:tc:SAML:1.0:am:unspecified", "xmlns", "Resource", "UserName", "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName", "EmailName", 
        "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress", "u", "ChannelInstance", "http://schemas.microsoft.com/ws/2005/02/duplex", "Encoding", "MimeType", "CarriedKeyName", "Recipient", "EncryptedKey", "KeyReference", "e", "http://www.w3.org/2001/04/xmlenc#Element", "http://www.w3.org/2001/04/xmlenc#Content", "KeyName", "MgmtData", "KeyValue", 
        "RSAKeyValue", "Modulus", "Exponent", "X509Data", "X509IssuerSerial", "X509IssuerName", "X509SerialNumber", "X509Certificate", "AckRequested", "http://schemas.xmlsoap.org/ws/2005/02/rm/AckRequested", "AcksTo", "Accept", "CreateSequence", "http://schemas.xmlsoap.org/ws/2005/02/rm/CreateSequence", "CreateSequenceRefused", "CreateSequenceResponse", 
        "http://schemas.xmlsoap.org/ws/2005/02/rm/CreateSequenceResponse", "FaultCode", "InvalidAcknowledgement", "LastMessage", "http://schemas.xmlsoap.org/ws/2005/02/rm/LastMessage", "LastMessageNumberExceeded", "MessageNumberRollover", "Nack", "netrm", "Offer", "r", "SequenceFault", "SequenceTerminated", "TerminateSequence", "http://schemas.xmlsoap.org/ws/2005/02/rm/TerminateSequence", "UnknownSequence", 
        "http://schemas.microsoft.com/ws/2006/02/tx/oletx", "oletx", "OleTxTransaction", "PropagationToken", "http://schemas.xmlsoap.org/ws/2004/10/wscoor", "wscoor", "CreateCoordinationContext", "CreateCoordinationContextResponse", "CoordinationContext", "CurrentContext", "CoordinationType", "RegistrationService", "Register", "RegisterResponse", "ProtocolIdentifier", "CoordinatorProtocolService", 
        "ParticipantProtocolService", "http://schemas.xmlsoap.org/ws/2004/10/wscoor/CreateCoordinationContext", "http://schemas.xmlsoap.org/ws/2004/10/wscoor/CreateCoordinationContextResponse", "http://schemas.xmlsoap.org/ws/2004/10/wscoor/Register", "http://schemas.xmlsoap.org/ws/2004/10/wscoor/RegisterResponse", "http://schemas.xmlsoap.org/ws/2004/10/wscoor/fault", "ActivationCoordinatorPortType", "RegistrationCoordinatorPortType", "InvalidState", "InvalidProtocol", "InvalidParameters", "NoActivity", "ContextRefused", "AlreadyRegistered", "http://schemas.xmlsoap.org/ws/2004/10/wsat", "wsat", 
        "http://schemas.xmlsoap.org/ws/2004/10/wsat/Completion", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Durable2PC", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Volatile2PC", "Prepare", "Prepared", "ReadOnly", "Commit", "Rollback", "Committed", "Aborted", "Replay", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Commit", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Rollback", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Committed", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Aborted", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Prepare", 
        "http://schemas.xmlsoap.org/ws/2004/10/wsat/Prepared", "http://schemas.xmlsoap.org/ws/2004/10/wsat/ReadOnly", "http://schemas.xmlsoap.org/ws/2004/10/wsat/Replay", "http://schemas.xmlsoap.org/ws/2004/10/wsat/fault", "CompletionCoordinatorPortType", "CompletionParticipantPortType", "CoordinatorPortType", "ParticipantPortType", "InconsistentInternalState", "mstx", "Enlistment", "protocol", "LocalTransactionId", "IsolationLevel", "IsolationFlags", "Description", 
        "Loopback", "RegisterInfo", "ContextId", "TokenId", "AccessDenied", "InvalidPolicy", "CoordinatorRegistrationFailed", "TooManyEnlistments", "Disabled", "ActivityId", "http://schemas.microsoft.com/2004/09/ServiceModel/Diagnostics", "http://docs.oasis-open.org/wss/oasis-wss-kerberos-token-profile-1.1#Kerberosv5APREQSHA1", "http://schemas.xmlsoap.org/ws/2002/12/policy", "FloodMessage", "LinkUtility", "Hops", 
        "http://schemas.microsoft.com/net/2006/05/peer/HopCount", "PeerVia", "http://schemas.microsoft.com/net/2006/05/peer", "PeerFlooder", "PeerTo", "http://schemas.microsoft.com/ws/2005/05/routing", "PacketRoutable", "http://schemas.microsoft.com/ws/2005/05/addressing/none", "http://schemas.microsoft.com/ws/2005/05/envelope/none", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/2001/XMLSchema", "nil", "type", "char", "boolean", "byte", 
        "unsignedByte", "short", "unsignedShort", "int", "unsignedInt", "long", "unsignedLong", "float", "double", "decimal", "dateTime", "string", "base64Binary", "anyType", "duration", "guid", 
        "anyURI", "QName", "time", "date", "hexBinary", "gYearMonth", "gYear", "gMonthDay", "gDay", "gMonth", "integer", "positiveInteger", "negativeInteger", "nonPositiveInteger", "nonNegativeInteger", "normalizedString", 
        "ConnectionLimitReached", "http://schemas.xmlsoap.org/soap/envelope/", "actor", "faultcode", "faultstring", "faultactor", "detail"
     };

        // Properties
        public int Count
        {
            get
            {
                return Strings.Length;
            }
        }

        public string this[int index]
        {
            get
            {
                return Strings[index];
            }
        }
    }

    internal static class IntEncoder
    {
        // Fields
        public const int MaxEncodedSize = 5;

        // Methods
        public static int Encode(int value, byte[] bytes, int offset)
        {
            int num = 1;
            while ((value & 0xffffff80L) != 0L)
            {
                bytes[offset++] = (byte)((value & 0x7f) | 0x80);
                num++;
                value = value >> 7;
            }
            bytes[offset] = (byte)value;
            return num;
        }

        public static int GetEncodedSize(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "ValueMustBeNonNegative");
            }
            int num = 1;
            while ((value & 0xffffff80L) != 0L)
            {
                num++;
                value = value >> 7;
            }
            return num;
        }
    }

    internal struct IntDecoder
    {
        private const int LastIndex = 4;
        private int value;
        private short index;
        private bool isValueDecoded;
        public int Value
        {
            get
            {
                if (!this.isValueDecoded)
                {
                    throw new InvalidOperationException("FramingValueNotAvailable");
                }
                return this.value;
            }
        }
        public bool IsValueDecoded
        {
            get
            {
                return this.isValueDecoded;
            }
        }
        public void Reset()
        {
            this.index = 0;
            this.value = 0;
            this.isValueDecoded = false;
        }

        public int Decode(byte[] buffer, int offset, int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "ValueMustBePositive");
            }
            if (this.isValueDecoded)
            {
                throw new InvalidOperationException("FramingValueNotAvailable");
            }
            int num = 0;
            while (num < size)
            {
                int num2 = buffer[offset];
                this.value |= (num2 & 0x7f) << (this.index * 7);
                num++;
                if ((this.index == 4) && ((num2 & 0xf8) != 0))
                {
                    throw new Exception("FramingSizeTooLarge");
                }
                this.index = (short)(this.index + 1);
                if ((num2 & 0x80) == 0)
                {
                    this.isValueDecoded = true;
                    return num;
                }
                offset++;
            }
            return num;
        }
    }


    #endregion

}
