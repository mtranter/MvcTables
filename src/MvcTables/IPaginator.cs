namespace MvcTables
{
    #region

    using System.Collections.Generic;

    #endregion

    internal interface IPaginator
    {
        Page First { get; }
        Page Last { get; }
        Page Previous { get; }
        Page Next { get; }
        Page Current { get; }
        IEnumerable<Page> Pages { get; }
    }
}