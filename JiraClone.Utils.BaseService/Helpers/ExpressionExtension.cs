using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.BaseService.Helpers
{
    public static class ExpressionExtension
    {
        public static IQueryable<TDto> WhereMany<TDto>(
            this IQueryable<TDto> query,
            IEnumerable<Expression<Func<TDto, bool>>> predicates)
        {
            if (predicates == null || !predicates.Any())
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(TDto), "dto");
            Expression combinedExpression = null;

            foreach (var predicate in predicates)
            {
                var visitor = new ReplaceExpressionVisitor(predicate.Parameters[0], parameter);
                var expression = visitor.Visit(predicate.Body);

                combinedExpression = combinedExpression == null
                    ? expression
                    : Expression.AndAlso(combinedExpression, expression);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<TDto, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}
