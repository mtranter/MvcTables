using System;
using System.IO;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcTables
{
    class RazorColumn<TModel> : ColumnBase<TModel>
    {
        private readonly string _columnTitle;
        private readonly Func<TModel, HelperResult> _template;

        public RazorColumn(string columnTitle, Func<TModel, HelperResult> template)
        {
            _columnTitle = columnTitle;
            _template = template;
        }

        public override MvcHtmlString GetCellValue(TModel[] model, int rowIndex, ControllerContext context, TextWriter textWriter)
        {
            return MvcHtmlString.Create(_template(model[rowIndex]).ToHtmlString());
        }

        public override MvcHtmlString GetHeaderValue(ControllerContext context, TextWriter textWriter)
        {
            return new MvcHtmlString(_columnTitle);
        }
    }
}