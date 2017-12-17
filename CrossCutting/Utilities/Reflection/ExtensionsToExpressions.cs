using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public static class ExtensionsToExpressions
	{
		public static ParameterExpression ToParameterExpression(this ParameterInfo parameterInfo)
		{
			return Expression.Parameter(parameterInfo.ParameterType, parameterInfo.Name ?? "x");
		}

		public static ParameterExpression ToParameterExpression(this ParameterInfo parameterInfo, string name)
		{
			return Expression.Parameter(parameterInfo.ParameterType, parameterInfo.Name ?? name);
		}

		public static IEnumerable<ParameterExpression> ToParameterExpressions(this IEnumerable<ParameterInfo> parameters)
		{
			return parameters.Select((parameter, index) => ToParameterExpression(parameter, "arg" + index));
		}

		public static IEnumerable<Expression> ToArrayIndexParameters(this IEnumerable<ParameterInfo> parameters, ParameterExpression arguments)
		{
			Func<ParameterInfo, int, Expression> converter = (parameter, index) =>
				{
					BinaryExpression arrayExpression = Expression.ArrayIndex(arguments, Expression.Constant(index));

					if (parameter.ParameterType.IsValueType)
						return Expression.Convert(arrayExpression, parameter.ParameterType);

					return Expression.TypeAs(arrayExpression, parameter.ParameterType);
				};

			return parameters.Select(converter);
		}
	}
}