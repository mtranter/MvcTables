namespace MvcTables
{
    #region

    using System;

    #endregion

    public static class TypeExtensions
    {
        public static bool CanSerializeToString(this Type type)
        {
            return type.IsNumeric() || type == typeof (string) || type == typeof (DateTime) || type == typeof (bool) ||
                   (type.IsNulalble() && type.GetGenericArguments()[0].CanSerializeToString());
        }

        public static bool IsNulalble(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        public static bool IsNumeric(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }
    }
}