using System;
using System.Linq.Expressions;

namespace Indigo.CrossCutting.Utilities.Reflection
{

	/// <summary>
	/// Use to safely set a property on an object, including the expansion of any lists as necessary
	/// and creation of reference properties. May not be suitable to all situations, but works great
	/// for deserializing data into an empty object graph.
	/// </summary>
	public class SafeProperty
	{
		readonly Action<object, int, object> _setter;

		SafeProperty(Type type, Action<object, int, object> setter)
		{
			Type = type;
			_setter = setter;
		}

		public Type Type { get; private set; }

		public void Set(object obj, int occurrence, object value)
		{
			_setter(obj, occurrence, value);
		}

		public static SafeProperty Create(Expression expression)
		{
			var visitor = new SafePropertyVisitor(expression);

			return new SafeProperty(visitor.Type, visitor.Setter);
		}
	}
}