using System;
using System.Linq.Expressions;

namespace Indigo.CrossCutting.Utilities.Reflection
{
    public class CurryExpressionVisitor<T1, T2, TResult> :
        ExpressionVisitor
    {
        ConstantExpression _replace;
        ParameterExpression _search;

        public Expression<Func<T1, TResult>> Curry(Expression<Func<T1, T2, TResult>> expression, T2 value)
        {
            _replace = Expression.Constant(value, typeof(int));
            _search = expression.Parameters[1];

            var result = Visit(expression) as LambdaExpression;
            if (result == null)
                throw new InvalidOperationException("Unable to curry expression: " + expression);

            Expression<Func<T1, TResult>> response = Expression.Lambda<Func<T1, TResult>>(result.Body,
                                                                                          result.Parameters[0]);

            return response;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (p == _search)
                return _replace;

            return base.VisitParameter(p);
        }
    }
}