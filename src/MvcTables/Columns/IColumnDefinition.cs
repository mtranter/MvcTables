namespace MvcTables
{
    #region

    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;

    #endregion

    interface IColumnDefinition<in TModel>
    {
        bool ShowRawValue { get; }
        IDictionary<string, object> HeaderAttributes { get; }
        IDictionary<string, object> CellAttributes { get; }
        IDictionary<string, object> FooterAttributes { get; }
        string Name { get; }
        string SortExpression { get; }
        bool IsHidden { get; }
        bool IsSortable { get; }
        int Index { get; }
        MvcHtmlString GetCellValue(TModel[] model, int rowIndex, ControllerContext context, TextWriter textWriter);
        MvcHtmlString GetHeaderValue(ControllerContext context, TextWriter textWriter);
        MvcHtmlString GetFooterValue(IEnumerable<TModel> rows, ControllerContext context, TextWriter textWriter);
        object GetRawValue(TModel model);
    }
}