namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    #endregion

    internal static class EnumerableExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> queryable, string expression, bool ascending)
        {
            if (String.IsNullOrEmpty(expression))
            {
                return queryable;
            }
            var lambda = StringToLambda<T>(expression);
            var orderByMethod = GetSortMethod<T>(typeof (Queryable), lambda.ReturnType, ascending);

            return (IQueryable<T>) orderByMethod.Invoke(null, new object[] {queryable, lambda});
        }

        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> queryable, string expression, bool ascending)
        {
            if (String.IsNullOrEmpty(expression))
            {
                return queryable;
            }
            var lambda = StringToLambda<T>(expression);
            var orderByMethod = GetSortMethod<T>(typeof (Enumerable), lambda.ReturnType, ascending);

            return (IEnumerable<T>) orderByMethod.Invoke(null, new object[] {queryable, lambda.Compile()});
        }

        private static MethodInfo GetSortMethod<T>(Type enumOrQueryable, Type returnType, bool ascending)
        {
            var methodName = ascending ? "OrderBy" : "OrderByDescending";

            // ReSharper disable PossibleNullReferenceException
            return enumOrQueryable.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                  .FirstOrDefault(
                                                  mi =>
                                                  mi.Name == methodName && mi.GetParameters().Length == 2)
                // ReSharper restore PossibleNullReferenceException
                                  .MakeGenericMethod(typeof (T), returnType);
        }

        private static LambdaExpression StringToLambda<T>(string expression)
        {
            var members = expression.Split('.');
            var param = Expression.Parameter(typeof (T));
            var body = members.Aggregate<string, Expression>(param, ParseMembers);
            var lambda = Expression.Lambda(body, param);
            return lambda;
        }

        private static Expression ParseMembers(Expression parent, string member)
        {
            var arrayAccess = new Regex(@"(\w+)\[(\d+)\]");
            var match = arrayAccess.Match(member);
            if (match.Success)
            {
                var indexType = typeof (int);
                int arrayIndex;
                if (!Int32.TryParse(match.Groups[2].Value, out arrayIndex))
                {
                    indexType = typeof (string);
                }

                var propertyExpression = Expression.Property(parent, match.Groups[1].Value);

                var indexer = (from p in propertyExpression.Type.GetDefaultMembers().OfType<PropertyInfo>()
                               let q = p.GetIndexParameters()
                               where q.Length == 1 && q[0].ParameterType == indexType
                               select p).Single();

                return propertyExpression.Type.IsArray
                           ? Expression.ArrayAccess(propertyExpression, Expression.Constant(arrayIndex))
                           : Expression.Property(propertyExpression, indexer, new[] {Expression.Constant(arrayIndex)});
            }

            return Expression.Property(parent, member);
        }
    }
}