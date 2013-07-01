namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    public interface IColumnConfiguration<TModel> : IFluentInterface
    {
        IColumnConfiguration<TModel> SetName(string name);
        IColumnConfiguration<TModel> SetIndex(int index);
        IColumnConfiguration<TModel> SetHeaderText(string headerText);
        IColumnConfiguration<TModel> Hide();
        IColumnConfiguration<TModel> IsSortable(bool sortable);
        IColumnConfiguration<TModel> SetCellCssClass(string @class);
        IColumnConfiguration<TModel> SetHeaderCssClass(string @class);
        IColumnConfiguration<TModel> SetFooterCssClass(string @class);
        IColumnConfiguration<TModel> SetCellAttributes(object attributes);
        IColumnConfiguration<TModel> SetCellAttributes(IDictionary<string, object> attributes);
        IColumnConfiguration<TModel> SetHeaderAttributes(object attributes);
        IColumnConfiguration<TModel> SetHeaderAttributes(IDictionary<string, object> attributes);
        IColumnConfiguration<TModel> SetFooterAttributes(object attributes);
        IColumnConfiguration<TModel> SetFooterAttributes(IDictionary<string, object> attributes);

        IColumnConfiguration<TModel> DefineFooter(
            Func<HtmlHelper<IEnumerable<TModel>>, IEnumerable<TModel>, string> footerDef);

        IColumnConfiguration<TModel> ConfigureRawData(Expression<Func<TModel, dynamic>> dataValue);
        IColumnConfiguration<TModel> RenderRawData(bool showInRawMode);

        IColumnConfiguration<TModel> SetSortProperty<TProperty>(Expression<Func<TModel, TProperty>> sortColumn)
            where TProperty : IComparable<TProperty>;
    }
}