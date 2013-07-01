namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Render;

    #endregion

    internal abstract class StringTableRenderBase<TModel> : ITableRender<TModel>
    {
        private static readonly Type[] TypesToWrap = new[] {typeof (string), typeof (char), typeof (DateTime)};
        private readonly ITableDefinition<TModel> _tableDefinition;

        internal StringTableRenderBase(ITableDefinition<TModel> tableDefinition)
        {
            _tableDefinition = tableDefinition;
        }

        protected abstract string ContentType { get; }
        protected abstract string BeginColumnsValue { get; }
        protected abstract string EndColumnsValue { get; }
        protected abstract string BeginRowsValue { get; }
        protected abstract string EndRowsValue { get; }
        protected abstract string ClosingValue { get; }
        protected abstract string ValueDelimiter { get; }
        protected abstract string BeginRowValue { get; }
        protected abstract string EndRowValue { get; }

        public void Render(IEnumerable<TModel> rows, TableRequestModel model,
                           ControllerContext context)
        {
            context.HttpContext.Response.ContentType = ContentType;
            var columns = _tableDefinition.Columns.Where(c => c.ShowRawValue).ToArray();
            var writer = new StreamWriter(context.HttpContext.Response.OutputStream);
            writer.Write(BeginColumnsValue);
            for (var i = 0; i < columns.Length; i++)
            {
                writer.Write(WrapText(columns[i].Name));
                if (i < (columns.Length - 1))
                {
                    writer.Write(ValueDelimiter);
                }
            }
            writer.Write(EndColumnsValue);
            writer.Write(BeginRowsValue);
            var aRows = rows.ToArray();
            for (var r = 0; r < aRows.Length; r++)
            {
                var row = aRows[r];
                writer.Write(BeginRowValue);
                for (var i = 0; i < columns.Length; i++)
                {
                    writer.Write(WrapText(columns[i].GetRawValue(row)));
                    if (i < (columns.Length - 1))
                    {
                        writer.Write(ValueDelimiter);
                    }
                }
                writer.Write(EndRowValue);
            }
            writer.Write(EndRowsValue);
            writer.Write(ClosingValue);
            writer.Flush();
        }

        private string WrapText(object value)
        {
            if (value == null)
            {
                return "";
            }
            var objType = value.GetType();
            if (TypesToWrap.Contains(objType) ||
                objType.IsNulalble() && TypesToWrap.Contains(Nullable.GetUnderlyingType(objType)))
            {
                return @"""" + (value ?? "") + @"""";
            }
            return (value ?? "").ToString();
        }
    }
}