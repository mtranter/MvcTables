namespace MvcTables
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    public static class Reflect
    {
        public static MemberInfo GetMember(Expression<Action> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(
                    GetMember(() => expression).Name);
            }

            return GetMemberInfo(expression);
        }

        public static MemberInfo GetMember<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(
                    GetMember(() => expression).Name);
            }

            return GetMemberInfo(expression);
        }

        public static MethodInfo GetMethod(Expression<Action> expression)
        {
            var method = GetMember(expression) as MethodInfo;
            if (method == null)
            {
                throw new ArgumentException(
                    "Not a method call expression", GetMember(() => expression).Name);
            }

            return method;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T>> expression)
        {
            var property = GetMember(expression) as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    "Not a property expression", GetMember(() => expression).Name);
            }

            return property;
        }

        public static PropertyInfo GetProperty<TObj, TProp>(Expression<Func<TObj, TProp>> expression)
        {
            var property = GetMemberInfo(expression) as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    "Not a property expression", GetMember(() => expression).Name);
            }

            return property;
        }

        public static FieldInfo GetField<T>(Expression<Func<T>> expression)
        {
            var field = GetMember(expression) as FieldInfo;
            if (field == null)
            {
                throw new ArgumentException(
                    "Not a field expression", GetMember(() => expression).Name);
            }

            return field;
        }

        internal static MemberInfo GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
            {
                throw new ArgumentNullException(
                    GetMember(() => lambda).Name);
            }

            MemberExpression memberExpression = null;
            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression) lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = lambda.Body as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.Call)
            {
                return ((MethodCallExpression) lambda.Body).Method;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", GetMember(() => lambda).Name);
            }

            return memberExpression.Member;
        }
    }

    public static class ReflectOn<T>
    {
        public static MemberInfo GetMember(Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(Reflect.GetMember(() => expression).Name);
            }

            return Reflect.GetMemberInfo(expression);
        }

        public static MemberInfo GetMember<TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(Reflect.GetMember(() => expression).Name);
            }

            return Reflect.GetMemberInfo(expression);
        }

        public static MethodInfo GetMethod(Expression<Action<T>> expression)
        {
            var method = GetMember(expression) as MethodInfo;
            if (method == null)
            {
                throw new ArgumentException(
                    "Not a method call expression",
                    Reflect.GetMember(() => expression).Name);
            }

            return method;
        }

        public static PropertyInfo GetProperty<TResult>(Expression<Func<T, TResult>> expression)
        {
            var property = GetMember(expression) as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    "Not a property expression", Reflect.GetMember(() => expression).Name);
            }

            return property;
        }

        public static FieldInfo GetField<TResult>(Expression<Func<T, TResult>> expression)
        {
            var field = GetMember(expression) as FieldInfo;
            if (field == null)
            {
                throw new ArgumentException(
                    "Not a field expression", Reflect.GetMember(() => expression).Name);
            }

            return field;
        }
    }
}