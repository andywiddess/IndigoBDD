using System;
using System.Linq.Expressions;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public interface IFastInvoker
	{
	}

	public interface IFastInvoker<T> :
		IFastInvoker
	{
		void FastInvoke(T target, Expression<Action<T>> expression);
		void FastInvoke(T target, Expression<Action<T>> expression, params object[] args);
		void FastInvoke(T target, Type[] genericTypes, Expression<Action<T>> expression);
		void FastInvoke(T target, Type[] genericTypes, Expression<Action<T>> expression, params object[] args);

		void FastInvoke(T target, string methodName);
		void FastInvoke(T target, string methodName, params object[] args);
		void FastInvoke(T target, Type[] genericTypes, string methodName);
		void FastInvoke(T target, Type[] genericTypes, string methodName, params object[] args);
		Action<T> GetInvoker(string methodName);
		Action<T, object[]> GetInvoker(string methodName, object[] args);
	}

	public interface IFastInvoker<T, TResult>
	{
		TResult FastInvoke(T target, string methodName);
		TResult FastInvoke(T target, string methodName, params object[] args);
		TResult FastInvoke(T target, Type[] genericTypes, string methodName);
		TResult FastInvoke(T target, Type[] genericTypes, string methodName, params object[] args);

		TResult FastInvoke(T target, Expression<Func<T,TResult>> expression);
		TResult FastInvoke(T target, Expression<Func<T, TResult>> expression, params object[] args);
		TResult FastInvoke(T target, Type[] genericTypes, Expression<Func<T, TResult>> expression);
		TResult FastInvoke(T target, Type[] genericTypes, Expression<Func<T, TResult>> expression, params object[] args);
	}
}