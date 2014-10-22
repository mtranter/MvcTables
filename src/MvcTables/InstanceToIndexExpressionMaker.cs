using System.Linq;

namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    ///     Used to turn expressions of type (TModel model) => model.Property
    ///     into expressions of tyoe (TModel[] model) => model[i].Property.
    ///     Used so HtmlHelpers generate Form compatible 'name' attributes for &lt;input/&gt; elements
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal class InstanceToIndexExpressionMaker<TModel> : ExpressionVisitor
    {
        public static Expression<Func<TModel[], TColumn>> Replace<TColumn>(int index, Expression<Func<TModel, TColumn>> lambdaExpression)
        {
            var visitor = new ExpressionReplacementVisitor(index, lambdaExpression);
            var lambda = visitor.Visit(lambdaExpression);
            return (Expression<Func<TModel[], TColumn>>) lambda;
        }

        class ExpressionReplacementVisitor : ExpressionVisitor
        {
          
            private readonly ParameterExpression _newParam, _oldParam;
            private readonly BinaryExpression _replacement;

            public ExpressionReplacementVisitor(int index, LambdaExpression expression)
            {
        
                _newParam = Expression.Parameter(typeof(TModel[]));
                _replacement = Expression.ArrayIndex(_newParam, Expression.Constant(index));
                _oldParam = expression.Parameters.First();
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var ex = (LambdaExpression)base.VisitLambda<T>(node);

                return Expression.Lambda(ex.Body, _newParam);
            }



            protected override Expression VisitIndex(IndexExpression node)
            {
                if (node.Object == _oldParam)
                {
                    return Expression.MakeIndex(_replacement, node.Indexer, node.Arguments);
                }

                return base.VisitIndex(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Object == _oldParam)
                {
                    return Expression.Call(_replacement, node.Method, node.Arguments);
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression == _oldParam)
                {
                    return Expression.PropertyOrField(_replacement, node.Member.Name);
                }

                return base.VisitMember(node);
            }

            
        }

    }
}