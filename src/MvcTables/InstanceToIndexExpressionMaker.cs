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
        private readonly Stack<Func<Expression, Expression>> _builders = new Stack<Func<Expression, Expression>>();
        private readonly int _index;

        public InstanceToIndexExpressionMaker(int index)
        {
            _index = index;
        }

        public override Expression Visit(Expression node)
        {
            var retval = base.Visit(node);
            return retval;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var newParameter = Expression.Parameter(typeof (TModel[]), "Model");
            base.VisitLambda(node);
            Expression head = Expression.ArrayIndex(newParameter, Expression.Constant(_index));

            do
            {
                var func = _builders.Pop();
                head = func(head);
            } while (_builders.Count > 0);

            return Expression.Lambda(head, newParameter);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            _builders.Push(e => Expression.MakeIndex(e, node.Indexer, node.Arguments));
            return base.VisitIndex(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            _builders.Push(e => Expression.Call(e, node.Method, node.Arguments));
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _builders.Push(e => Expression.Property(e, node.Member.Name));
            return base.VisitMember(node);
        }
    }
}