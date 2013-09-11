namespace MvcTables.Configuration
{
    #region

    using System;

    #endregion

    public interface IAssemblyConfiguration
    {
        /// <summary>
        /// Search the assembly that contains the type <typeparam name="TType"></typeparam>
        /// for <see cref="MvcTable{TTableModel}"/> configuration classes
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        void As<TType>();

        /// <summary>
        /// Search the assembly that contains the type <param name="type"></param>
        /// for <see cref="MvcTable{TTableModel}"/> configuration classes
        /// </summary>
        /// <param name="type"></param>
        void As(Type type);
    }
}