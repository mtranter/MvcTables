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

            while (_builders.Count > 0)
            {
                var func = _builders.Pop();
                head = func(head);
            }

            return Expression.Lambda(head, newParameter);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            if (ExpressionEvaluator.PartialEval(node).NodeType != ExpressionType.Constant)
            {
                _builders.Push(e => Expression.MakeIndex(e, node.Indexer, node.Arguments));
            }
            return base.VisitIndex(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (ExpressionEvaluator.PartialEval(node).NodeType != ExpressionType.Constant)
            {
                _builders.Push(e => Expression.Call(e, node.Method, node.Arguments));
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (ExpressionEvaluator.PartialEval(node).NodeType != ExpressionType.Constant)
            {
                _builders.Push(e => Expression.Property(e, node.Member.Name));
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            _builders.Push(e => Expression.Goto(node.Target));
            return base.VisitGoto(node);
        }

        
    }

    public static class ExpressionEvaluator
    {
        /// <summary>
        ///     Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }

        /// <summary>
        ///     Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, CanEvaluate);
        }

        private static bool CanEvaluate(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        ///     Performs bottom-up analysis to determine which nodes can possibly
        ///     be part of an evaluated sub-tree.
        /// </summary>
        private class Nominator : ExpressionVisitor
        {
            private readonly Func<Expression, bool> _fnCanBeEvaluated;
            private HashSet<Expression> _candidates;
            private bool _cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                _fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                _candidates = new HashSet<Expression>();
                Visit(expression);
                return _candidates;
            }

            public override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    var saveCannotBeEvaluated = _cannotBeEvaluated;
                    _cannotBeEvaluated = false;
                    base.Visit(expression);
                    if (!_cannotBeEvaluated)
                    {
                        if (_fnCanBeEvaluated(expression))
                        {
                            _candidates.Add(expression);
                        }
                        else
                        {
                            _cannotBeEvaluated = true;
                        }
                    }
                    _cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return expression;
            }
        }

        /// <summary>
        ///     Evaluates & replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        private class SubtreeEvaluator : ExpressionVisitor
        {
            private readonly HashSet<Expression> _candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                _candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return Visit(exp);
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }
                if (_candidates.Contains(exp))
                {
                    return Evaluate(exp);
                }
                return base.Visit(exp);
            }

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }
                var lambda = Expression.Lambda(e);
                var fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
        }
    }
}