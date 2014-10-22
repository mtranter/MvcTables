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
    internal class InstanceToIndexExpressionMaker<TModel> 
    {
        public static Expression<Func<TModel[], TColumn>> Replace<TColumn>(int index, Expression<Func<TModel, TColumn>> lambdaExpression)
        {
            var newParam = Expression.Parameter(typeof(TModel[]));
            var replacement = Expression.ArrayIndex(newParam, Expression.Constant(index));

            var visitor = new ExpressionReplacementVisitor(replacement, lambdaExpression.Parameters.First());
            
            
            var newBody = visitor.Visit(lambdaExpression.Body);

            return Expression.Lambda<Func<TModel[], TColumn>>(newBody, newParam);
        }

        class ExpressionReplacementVisitor : ExpressionVisitor
        {
          
            private readonly ParameterExpression _newParam, _oldParam;
            private readonly BinaryExpression _replacement;

            public ExpressionReplacementVisitor(BinaryExpression replacement, ParameterExpression oldParam)
            {
                _oldParam = oldParam;
                _replacement = replacement;

            }

            public override Expression Visit(Expression node)
            {
                var retval =  base.Visit(node);
                return retval;
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

            protected override Expression VisitGoto(GotoExpression node)
            {
                if (node.Value == _oldParam)
                {
                    return _replacement;
                }
                return base.VisitGoto(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParam)
                {
                    return _replacement;
                }
                return base.VisitParameter(node);
            }
            
        }

    }
}