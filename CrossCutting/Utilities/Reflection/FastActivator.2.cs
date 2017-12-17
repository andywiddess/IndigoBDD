using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Indigo.CrossCutting.Utilities.Reflection
{
	public class FastActivator<T, TArg0> :
		FastActivatorBase
	{
		[ThreadStatic]
		static FastActivator<T, TArg0> _current;

		Func<TArg0, T> _new;

		FastActivator()
			: base(typeof(T))
		{
			InitializeNew();
		}

		public static FastActivator<T, TArg0> Current
		{
			get
			{
				if (_current == null)
					_current = new FastActivator<T, TArg0>();

				return _current;
			}
		}

		void InitializeNew()
		{
			_new = arg0 =>
				{
					ConstructorInfo constructorInfo = Constructors
						.MatchingArguments(arg0)
						.SingleOrDefault();

					if (constructorInfo == null)
						throw new FastActivatorException(typeof(T), "No usable constructor found", typeof(TArg0));

					ParameterExpression parameter = constructorInfo.GetParameters().First().ToParameterExpression();

					Func<TArg0, T> lambda =
						Expression.Lambda<Func<TArg0, T>>(Expression.New(constructorInfo, parameter), parameter).Compile();

					_new = lambda;

					return lambda(arg0);
				};
		}

		public static T Create(TArg0 arg0)
		{
			return Current._new(arg0);
		}
	}
}