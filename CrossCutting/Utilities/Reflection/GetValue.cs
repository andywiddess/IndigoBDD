using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public delegate void GetValue<T>(Action<T> callback);
}