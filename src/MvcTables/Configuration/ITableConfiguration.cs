using System.Web.WebPages;

namespace MvcTables.Configuration
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion


    public interface IViewTableConfiguration<TModel> : IStaticTableConfiguration<TModel>
    {
        IViewTableConfiguration<TModel> AddRazorColumn(string columnTitle, Func<TModel, HelperResult> template,
                                                       Action<IColumnConfiguration<TModel>> columnConfiguration = null);
    }
    
    public interface IStaticTableConfiguration<TModel> : ITableConfiguration<TModel> 
    {
        /// <summary>
        /// Sets the Controller Action that will process the <see cref="TableRequestModel"/> 
        /// This method must be called by any class that inherits <see cref="MvcTable{TModel}"/>
        /// </summary>
        /// <param name="action">The name of the Action method</param>
        /// <param name="controller">The name of the Controller</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        IStaticTableConfiguration<TModel> SetAction(string action, string controller);

        /// <summary>
        /// Sets the Controller Action that will process the <see cref="TableRequestModel"/> 
        /// This method must be called by any class that inherits <see cref="MvcTable{TModel}"/>
        /// </summary>
        /// <param name="action">The name of the Action method</param>
        /// <param name="controller">The name of the Controller</param>
        /// <param name="area">The name of the area</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        IStaticTableConfiguration<TModel> SetAction(string action, string controller, string area);

        /// <summary>
        ///     Automatically scaffolds all columns with a DisplayFor() configuration
        /// </summary>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        IStaticTableConfiguration<TModel> ScaffoldAllColumns();
    }

    public interface ITableConfiguration<TModel> : IFluentInterface
    {
        /// <summary>
        /// Sets the deafult page size of the table
        /// </summary>
        /// <param name="pageSize">The number of rows</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> SetDefaultPageSize(int pageSize);

        /// <summary>
        /// Sets the Column that will be sorted by default and the sort order
        /// </summary>
        /// <param name="column">The name of the column to sory by</param>
        /// <param name="sortAscending">Set true to sort in ascending order</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> SetDefaultSortColumn(string column, bool sortAscending);

        /// <summary>
        /// Sets the Column that will be sorted by default and the sort order
        /// </summary>
        ///  /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="columnDefinition">A <see cref="Func{T}" /> used to set the default column to sort by</param>
        /// <param name="sortAscending">Set true to sort in ascending order</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> SetDefaultSortColumn<TColumn>(Expression<Func<TModel, TColumn>> columnDefinition, bool sortAscending);

        /// <summary>
        ///     Defines the CSS class(es) that decorate the &gt;table /&lt;
        /// </summary>
        /// <param name="class">The css class.</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> SetCssClass(string @class);

        /// <summary>
        ///     Defines the CSS class(es) that decorate the &gt;table /&lt;
        /// </summary>
        /// <param name="class">The css class.</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> SetFilterClass(string @class);

        /// <summary>
        ///     Configures the paging control
        /// </summary>
        /// <param name="pagingCfg">
        ///     The <see cref="Action{T}" /> used to configure the paging control
        /// </param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ConfigurePagingControl(Action<IPagingControlConfiguration> pagingCfg);
        
        /// <summary>
        ///     Adds a simple text column
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="columnDefinition">
        ///     A <see cref="Func{T}" /> used to generate the data rendered in this cell
        /// </param>
        /// <param name="columnConfig">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> AddColumn<TColumn>(Expression<Func<TModel, TColumn>> columnDefinition,
                                                       Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                           columnConfig = null);

        /// <summary>
        ///     Adds a column that uses Display Templates to render cell values
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="columnDefinition">
        ///     A <see cref="Func{T}" /> used to generate the data rendered in this cell
        /// </param>
        /// <param name="columnConfig">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> DisplayForColumn<TColumn>(Expression<Func<TModel, TColumn>> columnDefinition,
                                                              Action<IColumnConfiguration<TModel>> columnConfig = null);

        /// <summary>
        ///     Adds a column that uses Editor Templates to render cell values
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="columnDefinition">
        ///     A <see cref="Func{T}" /> used to generate the data rendered in this cell
        /// </param>
        /// <param name="columnConfig">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> EditorForColumn<TColumn>(Expression<Func<TModel, TColumn>> columnDefinition,
                                                             Action<IColumnConfiguration<TModel>> columnConfig = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="text">
        ///     The text to display derived from a property on the <typeparamref name="TModel" />
        /// </param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn<TColumn>(Expression<Func<TModel, TColumn>> text, string action,
                                                              Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                                  columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="text">
        ///     The text to display derived from a property on the <typeparamref name="TModel" />
        /// </param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn<TColumn>(Expression<Func<TModel, TColumn>> text, string action,
                                                              string controller,
                                                              Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                                  columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="text">
        ///     The text to display derived from a property on the <typeparamref name="TModel" />
        /// </param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn<TColumn>(Expression<Func<TModel, TColumn>> text, string action,
                                                              string controller, Func<TModel, object> routeValues,
                                                              Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                                  columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="text">
        ///     The text to display derived from a property on the <typeparamref name="TModel" />
        /// </param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="htmlAttributes">Attributes on the anchor tag</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn<TColumn>(Expression<Func<TModel, TColumn>> text, string action,
                                                              string controller, Func<TModel, object> routeValues,
                                                              object htmlAttributes,
                                                              Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                                  columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="text">
        ///     The text to display derived from a property on the <typeparamref name="TModel" />
        /// </param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="htmlAttributes">Attributes on the anchor tag</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn<TColumn>(Expression<Func<TModel, TColumn>> text, string action,
                                                              string controller, Func<TModel, object> routeValues,
                                                              IDictionary<string, object> htmlAttributes,
                                                              Action<IFormatableColumnConfiguration<TModel, TColumn>>
                                                                  columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn(string text, string action,
                                                     Action<IColumnConfiguration<TModel>> columnConfiguration = null);


        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn(string text, string action, string controller,
                                                     Action<IColumnConfiguration<TModel>> columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn(string text, string action, string controller,
                                                     Func<TModel, object> routeValues,
                                                     Action<IColumnConfiguration<TModel>> columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="htmlAttributes">Attributes on the anchor tag</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn(string text, string action, string controller,
                                                     Func<TModel, object> routeValues, object htmlAttributes,
                                                     Action<IColumnConfiguration<TModel>> columnConfiguration = null);

        /// <summary>
        ///     Renders an anchor tag.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="action">The name of the controller action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="routeValues">Route values to append to the anchor</param>
        /// <param name="htmlAttributes">Attributes on the anchor tag</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> ActionLinkColumn(string text, string action, string controller,
                                                     Func<TModel, object> routeValues,
                                                     IDictionary<string, object> htmlAttributes,
                                                     Action<IColumnConfiguration<TModel>> columnConfiguration = null);

        /// <summary>
        ///     Renders a HiddenInput into each cell. Note that the column itself will not be hidden.
        ///     To hide the column, user cfg => cfg.Hide() as the last parameter
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="property">The value for which to render the hidden input</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> HiddenColumnFor<TColumn>(Expression<Func<TModel, TColumn>> property,
                                                             Action<IColumnConfiguration<TModel>> columnConfiguration =
                                                                 null);

        /// <summary>
        ///     Renders a partial view for the given property
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="partialName">The partial to render</param>
        /// <param name="property">
        ///     An expression that defined the property that will be passed as the model to <paramref name="partialName" />
        /// </param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> PartialColumnFor<TColumn>(string partialName,
                                                              Expression<Func<TModel, TColumn>> property,
                                                              Action<IColumnConfiguration<TModel>> columnConfiguration =
                                                                  null);

        /// <summary>
        ///     Renders cells with dropdown controls
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <param name="property">An expression that defined the property that will be bound to the dropdown</param>
        /// <param name="listValues">The list of values to display in the dropdown</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> DropdownColumnFor<TColumn>(Expression<Func<TModel, TColumn>> property,
                                                               IEnumerable<SelectListItem> listValues,
                                                               Action<IColumnConfiguration<TModel>> columnConfiguration
                                                                   = null);

        /// <summary>
        ///     Renders cells with dropdown controls
        /// </summary>
        /// <typeparam name="TColumn">
        ///     The <see cref="System.Type" /> of this column
        /// </typeparam>
        /// <typeparam name="TKey">The type of the Value rendered in the Select option</typeparam>
        /// <typeparam name="TDisp">The type of the text rendered in the Select option</typeparam>
        /// <param name="property">An expression that defined the property that will be bound to the dropdown</param>
        /// <param name="listValues">The list of values to display in the dropdown</param>
        /// <param name="selectValue">A selector func used to render the value for each select option</param>
        /// <param name="displayValue">A selector func used to render the text for each select option</param>
        /// <param name="columnConfiguration">Used to override default column settings</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> DropdownColumnFor<TColumn, TKey, TDisp>(Expression<Func<TModel, TColumn>> property,
                                                                            IEnumerable<TColumn> listValues,
                                                                            Func<TColumn, TKey> selectValue,
                                                                            Func<TColumn, TDisp> displayValue,
                                                                            Action<IColumnConfiguration<TModel>>
                                                                                columnConfiguration = null);

        /// <summary>
        ///     Hide column from the table
        /// </summary>
        /// <param name="name">Name of the column to hide</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> HideColumn(string name);

        /// <summary>
        ///     Hide column from the table
        /// </summary>
        /// <param name="property">Expression selecting the column to hide</param>
        /// <returns>
        ///     An instance of <see cref="ITableConfiguration{TModel}" /> to allow chaining
        /// </returns>
        ITableConfiguration<TModel> HideColumn<TColumn>(Expression<Func<TModel, TColumn>> property);


        /// <summary>
        ///    List of columns that will not displayed
        /// </summary>
        IList<string> HiddenColumns { get; }
    }
}