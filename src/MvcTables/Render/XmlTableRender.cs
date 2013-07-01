namespace MvcTables.Render
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Linq;

    #endregion

    internal class XmlTableRender<TModel> : ITableRender<TModel>
    {
        private readonly ITableDefinition<TModel> _tableDefinition;

        public XmlTableRender(ITableDefinition<TModel> tableDefinition)
        {
            _tableDefinition = tableDefinition;
        }

        public void Render(IEnumerable<TModel> rows, TableRequestModel model, ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";
            var columns = new XElement("columns");
            columns.Add(_tableDefinition.Columns.Select(x => new XElement("column", x.Name)));
            var xrows = new XElement("rows");
            xrows.Add(
                      rows.Select(
                                  r =>
                                  new XElement("row",
                                               _tableDefinition.Columns.Select(
                                                                               t =>
                                                                               new XElement("cell", t.GetRawValue(r))))));
            var table = new XElement("table", columns, xrows);
            table.Save(context.HttpContext.Response.OutputStream);
        }
    }
}