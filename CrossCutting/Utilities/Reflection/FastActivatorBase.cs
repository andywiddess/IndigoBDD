using System;
using System.Reflection;


namespace Indigo.CrossCutting.Utilities.Reflection
{
	public abstract class FastActivatorBase
	{
		const BindingFlags _constructorBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		protected readonly ConstructorInfo[] Constructors;

		protected FastActivatorBase(Type type)
		{
			ObjectType = type;
			Constructors = type.GetConstructors(_constructorBindingFlags);
		}

		protected Type ObjectType { get; set; }
	}
}