using System.Linq.Expressions;

namespace App.Common.Helpers.ExpressionExtentions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            // Replace parameters in the expressions with the same parameter
            var left = new ReplaceParameterVisitor(expr1.Parameters[0], parameter).Visit(expr1.Body);
            var right = new ReplaceParameterVisitor(expr2.Parameters[0], parameter).Visit(expr2.Body);

            // Combine the expressions with AndAlso
            var combined = Expression.AndAlso(left!, right!);

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : node;
            }
        }
    }
}
