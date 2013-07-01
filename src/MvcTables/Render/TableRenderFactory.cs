namespace MvcTables.Render
{
    #region

    using System;

    #endregion

    internal static class TableRenderFactory
    {
        public static ITableRender<TModel> Get<TModel>(ITableDefinition<TModel> config, TableRequestModel model,
                                                       ITableUrlManager urlManager, IPaginator paginator)
        {
            switch (model.Format)
            {
                case TableRenderFormat.Html:
                    return new HtmlTableRender<TModel>(config, urlManager, paginator);
                case TableRenderFormat.Json:
                    return new JsonArrayTableRender<TModel>(config);
                case TableRenderFormat.Xml:
                    return new XmlTableRender<TModel>(config);
                case TableRenderFormat.Csv:
                    return new CsvTableRender<TModel>(config);
            }

            throw new Exception("Unknown table render type");
        }
    }
}