using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public class FastActivatorException :
		Exception
	{
		public FastActivatorException()
		{
		}

		public FastActivatorException(Type type, string message, params Type[] argumentTypes)
			: base(FormatMessage(type, message, argumentTypes))
		{
			Type = type;
			ArgumentTypes = new List<Type>(argumentTypes);
		}

		public FastActivatorException(Type type, string message, Exception innerException, params Type[] argumentTypes)
			: base(FormatMessage(type, message, argumentTypes), innerException)
		{
			Type = type;
			ArgumentTypes = new List<Type>(argumentTypes);
		}

		protected FastActivatorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public Type Type { get; set; }
		public IEnumerable<Type> ArgumentTypes { get; set; }

		private static string FormatMessage(Type type, string message, Type[] argumentTypes)
		{
			return string.Format("An object of type '{0}' could not be created. '{1}' that takes '{2}'", type.Name, message,
				string.Join(", ", argumentTypes.Select(x => x.Name).ToArray()));
		}
	}
}