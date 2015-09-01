using System.Web.WebPages;

namespace MvcTables.Configuration
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    internal class TableConfiguration<TModel> : IStaticTableConfiguration<TModel>, ITableDefinition<TModel>, IViewTableConfiguration<TModel>
    {

        private List<string> _hiddenColumns;

        private static readonly MethodInfo AddColumnMethod =
            typeof (ITableConfiguration<TModel>).GetMethod("DisplayForColumn");

        private readonly ColumnCollection<TModel> _columns = new ColumnCollection<TModel>();

        public TableConfiguration()
        {
            PagingConfiguration = new PagingControlConfiguration(this);
            _hiddenColumns = new List<string>();
            FilterExpression = "mvc-table-filter";
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; private set; }

        public string CssClass { get; private set; }

        public IEnumerable<IColumnDefinition<TModel>> Columns
        {
            get { return _columns.AsEnumerable(); }
        }

        public PagingControlConfiguration PagingConfiguration { get; private set; }

        public string FilterExpression { get; private set; }

        public string Action { get; private set; }

        public string Controller { get; private set; }

        public string Area { get; private set; }

        


        public string DefaultSortColumn { get; private set; }

        public bool? DefaultSortAscending { get; private set; }

        public int? DefaultPageSize { get; private set; }


        #region ITableConfiguration<TModel> Members

        ITableConfiguration<TModel> ITableConfiguration<TModel>.SetDefaultPageSize(int pageSize)
        {
            DefaultPageSize = pageSize;
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.SetDefaultSortColumn(string column, bool sortAscending)
        {
            DefaultSortColumn = column;
            DefaultSortAscending = sortAscending;
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.SetDefaultSortColumn<TColumn>(Expression<Func<TModel, TColumn>> columnDefinition, bool sortAscending)
        {
            DefaultSortColumn = ExpressionHelper.GetExpressionText(columnDefinition);
            DefaultSortAscending = sortAscending;
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.SetCssClass(string @class)
        {
            CssClass = @class;
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.SetFilterClass(string @class)
        {
            FilterExpression = @class;
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ConfigurePagingControl(
            Action<IPagingControlConfiguration> pagingCfg)
        {
            pagingCfg(PagingConfiguration);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.AddColumn<TColumn>(
            Expression<Func<TModel, TColumn>> columnDefinition,
            Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfig)
        {
            var config = new TextColumn<TModel, TColumn>(columnDefinition);
            columnConfig.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.DisplayForColumn<TColumn>(
            Expression<Func<TModel, TColumn>> columnDefinition, Action<IColumnConfiguration<TModel>> columnConfig)
        {
            var config = new DisplayForColumn<TModel, TColumn>(columnDefinition);
            columnConfig.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.EditorForColumn<TColumn>(
            Expression<Func<TModel, TColumn>> columnDefinition, Action<IColumnConfiguration<TModel>> columnConfig)
        {
            var config = new EditorForColumn<TModel, TColumn>(columnDefinition);
            columnConfig.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn<TColumn>(
            Expression<Func<TModel, TColumn>> text, string action,
            Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, null, columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn<TColumn>(
            Expression<Func<TModel, TColumn>> text, string action, string controller,
            Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, null,
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn<TColumn>(
            Expression<Func<TModel, TColumn>> text, string action, string controller, Func<TModel, object> routeValues,
            Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, routeValues, null,
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn<TColumn>(
            Expression<Func<TModel, TColumn>> text, string action, string controller, Func<TModel, object> routeValues,
            object htmlAttributes, Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, routeValues,
                                                                          new RouteValueDictionary(htmlAttributes),
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn<TColumn>(
            Expression<Func<TModel, TColumn>> text, string action, string controller, Func<TModel, object> routeValues,
            IDictionary<string, object> htmlAttributes,
            Action<IFormatableColumnConfiguration<TModel, TColumn>> columnConfiguration)
        {
            var config = new ActionLinkForColumn<TModel, TColumn>(text, action, controller, routeValues ?? (r => null),
                                                                  htmlAttributes);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn(string text, string action,
                                                                                 Action<IColumnConfiguration<TModel>>
                                                                                     columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, null, columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn(string text, string action,
                                                                                 string controller,
                                                                                 Action<IColumnConfiguration<TModel>>
                                                                                     columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, null,
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn(string text, string action,
                                                                                 string controller,
                                                                                 Func<TModel, object> routeValues,
                                                                                 Action<IColumnConfiguration<TModel>>
                                                                                     columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, routeValues, null,
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn(string text, string action,
                                                                                 string controller,
                                                                                 Func<TModel, object> routeValues,
                                                                                 object htmlAttributes,
                                                                                 Action<IColumnConfiguration<TModel>>
                                                                                     columnConfiguration)
        {
            return (this as ITableConfiguration<TModel>).ActionLinkColumn(text, action, controller, routeValues,
                                                                          new RouteValueDictionary(htmlAttributes),
                                                                          columnConfiguration);
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.ActionLinkColumn(string text, string action,
                                                                                 string controller,
                                                                                 Func<TModel, object> routeValues,
                                                                                 IDictionary<string, object>
                                                                                     htmlAttributes,
                                                                                 Action<IColumnConfiguration<TModel>>
                                                                                     columnConfiguration)
        {
            var config = new StaticActionLinkColumn<TModel>(text, action, controller, routeValues, htmlAttributes);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.HiddenColumnFor<TColumn>(
            Expression<Func<TModel, TColumn>> property, Action<IColumnConfiguration<TModel>> columnConfiguration)
        {
            var config = new HiddenForColumn<TModel, TColumn>(property);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.PartialColumnFor<TColumn>(string partialName,
                                                                                          Expression
                                                                                              <Func<TModel, TColumn>>
                                                                                              property,
                                                                                          Action
                                                                                              <
                                                                                              IColumnConfiguration
                                                                                              <TModel>>
                                                                                              columnConfiguration)
        {
            var config = new PartialForColumn<TModel, TColumn>(property, partialName);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.DropdownColumnFor<TColumn>(
            Expression<Func<TModel, TColumn>> property, IEnumerable<SelectListItem> listValues,
            Action<IColumnConfiguration<TModel>> columnConfiguration)
        {
            var config = new DropdownForColumn<TModel, TColumn>(property, listValues);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }

        ITableConfiguration<TModel> ITableConfiguration<TModel>.DropdownColumnFor<TColumn, TKey, TDisp>(
            Expression<Func<TModel, TColumn>> property, IEnumerable<TColumn> listValues, Func<TColumn, TKey> selectValue,
            Func<TColumn, TDisp> displayValue, Action<IColumnConfiguration<TModel>> columnConfiguration = null)
        {
            var listItems =
                listValues.Select(
                                  i =>
                                  new SelectListItem
                                      {
                                          Text = displayValue(i).ToString(),
                                          Value = selectValue(i).ToString()
                                      });
            return (this as ITableConfiguration<TModel>).DropdownColumnFor(property, listItems, columnConfiguration);
        }



        public IEnumerable<string> HiddenColumns { get { return _hiddenColumns; } }

        public ITableConfiguration<TModel> HideColumn(string name)
        {
            _hiddenColumns.Add(name);
            return this;
        }

        public ITableConfiguration<TModel> HideColumn<TColumn>(Expression<Func<TModel, TColumn>> property)
        {
            _hiddenColumns.Add(ExpressionHelper.GetExpressionText(property));
            return this;
        }

        #endregion

        #region IStaticTableConfiguration<TModel> Members

        IStaticTableConfiguration<TModel> IStaticTableConfiguration<TModel>.SetAction(string action, string controller)
        {
            return (this as IStaticTableConfiguration<TModel>).SetAction(action, controller, null);
        }

        IStaticTableConfiguration<TModel> IStaticTableConfiguration<TModel>.SetAction(string action, string controller, string area)
        {
            Action = action;
            Controller = controller;
            Area = area;
            return this;
        }

        public IStaticTableConfiguration<TModel> ScaffoldAllColumns()
        {

            var properties = from p in typeof(TModel).GetProperties()
                             let attr = p.GetCustomAttribute<DisplayAttribute>()
                             where
                                 p.PropertyType.CanSerializeToString() &&
                                 (attr == null || attr.GetAutoGenerateField() != false)
                             select p;

            foreach (var p in properties)
            {
                var param = Expression.Parameter(typeof(TModel));
                var lambdaMethod = Expression.Lambda(Expression.Property(param, p), param);
                var addCol = AddColumnMethod.MakeGenericMethod(p.PropertyType);
                addCol.Invoke(this, new object[] { lambdaMethod, null });
            }

            return this;
        }

        #endregion

        public IViewTableConfiguration<TModel> AddRazorColumn(string columnTitle, Func<TModel, HelperResult> template, Action<IColumnConfiguration<TModel>> columnConfiguration = null)
        {
            var config = new RazorColumn<TModel>(columnTitle, template);
            columnConfiguration.ExecuteIfNotNull(config);
            _columns.Add(config);
            return this;
        }
    }
}