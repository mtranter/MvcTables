namespace MvcTables.Render
{
    #region

    using System.IO;
    using System.Web.Mvc;

    #endregion

    public class WriterHelper
    {
        public static TextWriter GetWriter(ControllerContext context)
        {
            context = context.IsChildAction ? context.ParentActionViewContext : context;
            return context.HttpContext.Response.Output;
        }
    }
}