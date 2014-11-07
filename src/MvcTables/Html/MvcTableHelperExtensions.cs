namespace MvcTables.Html
{
    #region

    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Configuration;

    #endregion

    public static class MvcTableHelperExtensions
    {
        public static MvcTableHelper<TModel> MvcTable<TModel>(this HtmlHelper helper, Expression<Func<MvcTable<TModel>>> tableDef)
        {
            var table = TableConfigurations.Configurations.Get<TModel>(tableDef.Body.Type);
            return new MvcTableHelper<TModel>(helper, table);
        }

        public static MvcHtmlString MvcTableScript(this HtmlHelper helper)
        {
            var resource =
                typeof (TableResult<,>).Assembly.GetManifestResourceStream(@"MvcTables.Scripts.MvcTable.jQuery.js");
            var reader = new StreamReader(resource);
            var retval = @"<script type=""text/javascript"">" + reader.ReadToEnd() + "</script>";
            return MvcHtmlString.Create(retval);
        }

        public static MvcTableHelper<TModel> MvcTable<TModel>(this HtmlHelper helper,
            Action<IViewTableConfiguration<TModel>> customize)
        {
            var config = TableConfigurations.Configurations.GetDefaultTableConfiguration<TModel>();
            customize(config);

            TableConfigurations.Configurations.Add<ViewConfigedMvcTable<TModel>, TModel>(config);

            return new MvcTableHelper<TModel>(helper, config);
        }
    }
}