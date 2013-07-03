namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    /// <summary>
    /// Configures a table column
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IColumnConfiguration<TModel> : IFluentInterface
    {

        /// <summary>
        /// Sets the name of the column. Generally for internal use, however may come
        /// in handy if using custom client side scripting
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetName(string name);


        /// <summary>
        /// Sets the zero based index of the column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetIndex(int index);

        /// <summary>
        /// Sets the header text.
        /// </summary>
        /// <param name="headerText">The header text.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetHeaderText(string headerText);


        /// <summary>
        /// Hides this column.
        /// </summary>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> Hide();

        /// <summary>
        /// Determines whether the specified sortable is sortable.
        /// </summary>
        /// <param name="sortable">if set to <c>true</c> [sortable].</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> IsSortable(bool sortable);


        /// <summary>
        /// Sets the cell CSS class on the th and tds for this column.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetCellCssClass(string @class);

        /// <summary>
        /// Sets the header CSS class.
        /// </summary>
        /// <param name="class">The CSS class.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetHeaderCssClass(string @class);

        /// <summary>
        /// Sets the footer CSS class.
        /// </summary>
        /// <param name="class">The CSS class.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetFooterCssClass(string @class);
        
        /// <summary>
        /// Sets the html attributes on the cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetCellAttributes(object attributes);

        /// <summary>
        /// Sets the html attributes on the cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetCellAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Sets the html attributes on the header cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetHeaderAttributes(object attributes);

        /// <summary>
        /// Sets the html attributes on the header cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetHeaderAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Sets the html attributes on the footer cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetFooterAttributes(object attributes);

        /// <summary>
        /// Sets the html attributes on the footer cell
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetFooterAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Defines the HTML content of the footer cell.
        /// </summary>
        /// <param name="footerDef">The footer def.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> DefineFooter(Func<HtmlHelper<IEnumerable<TModel>>, IEnumerable<TModel>, string> footerDef);

        /// <summary>
        /// Configures what will be rendered with non-HTML renderes (i.e. CSV, JSON)
        /// </summary>
        /// <param name="dataValue">The data value.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> ConfigureRawData(Expression<Func<TModel, dynamic>> dataValue);

        /// <summary>
        /// Defines whether or not this column is shown when rendering non-HTML tables (i.e. CSV, JSON)
        /// </summary>
        /// <param name="showInRawMode">if set to <c>true</c> [show in raw mode].</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> RenderRawData(bool showInRawMode);

        /// <summary>
        /// Sets the property on the <typeparamref name="TModel"/> that will be used to sort the table,
        /// when this column is used as the Sort Column,
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="sortColumn">The sort column property.</param>
        /// <returns>An instance of <see cref="IColumnConfiguration{TModel}"/> to allow chaining</returns>
        IColumnConfiguration<TModel> SetSortProperty<TProperty>(Expression<Func<TModel, TProperty>> sortColumn)
            where TProperty : IComparable<TProperty>;
    }
}