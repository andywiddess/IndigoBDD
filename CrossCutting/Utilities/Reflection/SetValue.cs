using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public delegate void SetValue<in T>(T value);
}