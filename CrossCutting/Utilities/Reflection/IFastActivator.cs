using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public interface IFastActivator
	{
		object Create();
		object Create(object[] args);
		object Create<TArg0>(TArg0 arg0);
		object Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1);
	}

	public interface IFastActivator<T> :
		IFastActivator
	{
		new T Create();
		new T Create(object[] args);
		new T Create<TArg0>(TArg0 arg0);
		new T Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1);
	}
}