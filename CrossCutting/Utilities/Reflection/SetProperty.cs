using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public delegate void SetProperty<in T, in TProperty>(T obj, TProperty value);


	public delegate void SetProperty1<in T, in TProperty>(T obj, int occurrence, TProperty value);
}