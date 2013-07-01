namespace MvcTables
{
    #region

    using System;
    using System.IO;
    using System.Web.Mvc;

    #endregion

    internal static class HtmlHelperMock
    {
        internal static HtmlHelper<TRow> GetHelper<TRow>(TRow row, ControllerContext cc, TextWriter tw)
        {
            return
                new HtmlHelper<TRow>(
                    new ViewContext(cc, new DummyView(), new ViewDataDictionary(row), new TempDataDictionary(), tw),
                    new DummyViewDataContainer<TRow>(row));
        }

        #region Dummy class for HtmlHelper mockup

        private class DummyView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
                return;
            }
        }

        private class DummyViewDataContainer<TRow> : IViewDataContainer
        {
            private readonly ViewDataDictionary _viewData;

            public DummyViewDataContainer(TRow row)
            {
                _viewData = new ViewDataDictionary(row);
            }

            public ViewDataDictionary ViewData
            {
                get { return _viewData; }
                set { throw new NotImplementedException(); }
            }
        }

        #endregion
    }
}