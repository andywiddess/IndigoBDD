using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public delegate void GetProperty<in T, TProperty>(T obj, Action<TProperty> callback);
}