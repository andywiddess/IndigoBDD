using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public class FastActivator<T, TArg0, TArg1> :
		FastActivatorBase
	{
		[ThreadStatic]
		static FastActivator<T, TArg0, TArg1> _current;

		Func<TArg0, TArg1, T> _new;

		FastActivator()
			: base(typeof(T))
		{
			InitializeNew();
		}

		public static FastActivator<T, TArg0, TArg1> Current
		{
			get
			{
				if (_current == null)
					_current = new FastActivator<T, TArg0, TArg1>();

				return _current;
			}
		}

		void InitializeNew()
		{
			_new = (arg0, arg1) =>
				{
					ConstructorInfo constructorInfo = Constructors
						.MatchingArguments(arg0, arg1)
						.SingleOrDefault();

					if (constructorInfo == null)
						throw new FastActivatorException(typeof(T), "No usable constructor found", typeof(TArg0), typeof(TArg1));

					ParameterExpression[] parameters = constructorInfo.GetParameters().ToParameterExpressions().ToArray();

					Func<TArg0, TArg1, T> lambda =
						Expression.Lambda<Func<TArg0, TArg1, T>>(Expression.New(constructorInfo, parameters), parameters).Compile();

					_new = lambda;

					return lambda(arg0, arg1);
				};
		}

		public static T Create(TArg0 arg0, TArg1 arg1)
		{
			return Current._new(arg0, arg1);
		}
	}
}