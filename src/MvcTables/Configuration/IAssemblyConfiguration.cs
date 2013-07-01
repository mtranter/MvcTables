namespace MvcTables.Configuration
{
    #region

    using System;

    #endregion

    public interface IAssemblyConfiguration
    {
        void As<TType>();
        void As(Type type);
    }
}