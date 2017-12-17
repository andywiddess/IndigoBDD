//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Runtime.Serialization;

//namespace Indigo.CrossCutting.Utilities.DataTypes
//{
//    /// <summary>
//    /// A Simple Property that stores the Document in 2 columns - Document Name and Document Content
//    /// </summary>
//    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
//    public class Template : IConvertible, IComparable
//    {
//        [DataMember]
//        public string Content { get; set; }

//        [DataMember]
//        public byte[] Query { get; set; }

//        public override string ToString()
//        {
//            return Content;
//        }

//        #region IConvertible Members

//        public TypeCode GetTypeCode()
//        {
//            throw new NotImplementedException();
//        }

//        public bool ToBoolean(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public byte ToByte(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public char ToChar(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public DateTime ToDateTime(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public decimal ToDecimal(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public double ToDouble(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public short ToInt16(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public int ToInt32(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public long ToInt64(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public sbyte ToSByte(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public float ToSingle(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public string ToString(IFormatProvider provider)
//        {
//            return this.ToString();
//        }

//        public object ToType(Type conversionType, IFormatProvider provider)
//        {
//            if (conversionType == typeof(Template))
//            {
//                return this;
//            }
//            else if (conversionType == typeof(string))
//            {
//                return this.ToString(provider);
//            }
//            throw new InvalidCastException();
//        }

//        public ushort ToUInt16(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public uint ToUInt32(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        public ulong ToUInt64(IFormatProvider provider)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        //public static object FromObject(object value)
//        //{
//        //    if (value is Document)
//        //    {
//        //        return (Document)value;
//        //    }
//        //    else if (value is string)
//        //    {
//        //        return value.ToString();
//        //    }
//        //    throw new InvalidCastException();
//        //}

//        #region IComparable Members

//        public int CompareTo(object obj)
//        {
//            if (obj == null)
//            {
//                return 1;
//            }
//            if (!(obj is Template))
//            {
//                throw new ArgumentException("Value was of type '" + obj.GetType().Name + "', not Templatet.");
//            }
//            Template templateToCompare = (Template)obj;
//            if (this.Content.CompareTo(templateToCompare.Content) != 0)
//            {
//                //If the content is not same then the objects are not the same
//                return 1;
//            }
//            else if (this.Query.LongLength.CompareTo(templateToCompare.Query.LongLength) != 0)
//            {
//                //If the query length is not same then the objects are not the same
//                return 1;
//            }
//            else if (Convert.ToBase64String(this.Query, Base64FormattingOptions.None).CompareTo(Convert.ToBase64String(templateToCompare.Query, Base64FormattingOptions.None)) != 0)
//            {
//                //The contents are different - so the objects are not the same
//                return 1;
//            }
//            else
//            {
//                //The Content and Query are same - so do not update
//                return 0;
//            }

//        }

//        #endregion
//    }
//}
