namespace MvcTables.Render
{
    #region

    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    internal interface ITableRender<in TModel>
    {
        void Render(IEnumerable<TModel> rows, TableRequestModel model, ControllerContext context);
    }
}