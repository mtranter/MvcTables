namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    internal class DropdownForColumn<TModel, TColumn> : ReflectedColumnBase<TModel, TColumn>
    {
        private readonly IEnumerable<SelectListItem> _listValues;

        public DropdownForColumn(Expression<Func<TModel, TColumn>> columnDefinition,
                                 IEnumerable<SelectListItem> listValues)
            : base(columnDefinition)
        {
            _listValues = listValues;
        }

        protected override MvcHtmlString GetCellValueCore(TModel[] model, int rowIndex, TColumn cellValue,
                                                          HtmlHelper<TModel[]> helper)
        {
            var localList = _listValues.ToArray();
            var selectedValue = base.ColumnDefinition.Compile()(model[rowIndex]);
            var selectedItem = localList.FirstOrDefault(l => l.Value == selectedValue.ToString());
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
            }
            var retval = helper.DropDownListFor(ArrayIndexExpression(model, rowIndex), localList);

            foreach (var item in _listValues)
            {
                item.Selected = false;
            }

            return retval;
        }
    }
}