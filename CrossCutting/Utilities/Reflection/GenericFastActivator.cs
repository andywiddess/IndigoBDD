using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Indigo.CrossCutting.Utilities.Extensions;


namespace Indigo.CrossCutting.Utilities.Reflection
{
	public class GenericFastActivator :
		FastActivatorBase,
		IFastActivator
	{
		readonly Dictionary<int, Func<object[], object>> _argGenerators;

		public GenericFastActivator(Type genericType)
			: base(genericType)
		{
			_argGenerators = new Dictionary<int, Func<object[], object>>();
		}

		object IFastActivator.Create()
		{
			throw new NotImplementedException();
		}

		object IFastActivator.Create(object[] args)
		{
			return CreateFromArgs(args);
		}

		object IFastActivator.Create<TArg0>(TArg0 arg0)
		{
			return CreateFromArgs(arg0);
		}

		object IFastActivator.Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
		{
			return CreateFromArgs(arg0, arg1);
		}

		object CreateFromArgs(params object[] args)
		{
			int key = GenerateTypeKey(args);

			Func<object[], object> generator = GetGenerator(key, args);

			return generator(args);
		}

		Func<object[], object> GetGenerator(int key, params object[] args)
		{
			return _argGenerators.Retrieve(key, () =>
				{
					ConstructorInfo constructorInfo = Constructors
						.MatchingArguments(args)
						.SingleOrDefault();

					if (constructorInfo == null)
						throw new FastActivatorException(ObjectType, "No usable constructor found");

					Type specializedType = constructorInfo.ToSpecializedType(args);

					constructorInfo = specializedType.GetConstructors().MatchingArguments(args).SingleOrDefault();

					if (constructorInfo == null)
						throw new FastActivatorException(specializedType, "Specialized constructor could not be used to build the object");

					ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");

					Expression[] parameters = constructorInfo.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();

					NewExpression newExpression = Expression.New(constructorInfo, parameters);

					Func<object[], object> lambda = Expression.Lambda<Func<object[], object>>(newExpression, argsParameter).Compile();

					return lambda;
				});
		}

		static int GenerateTypeKey(params object[] args)
		{
			int offset = 0;
			return args.Aggregate(0, (x, o) => x ^ (o.GetType().GetHashCode() << offset++));
		}
	}
}