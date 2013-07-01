namespace MvcTables
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal class Paginator : IPaginator
    {
        private readonly int _currentPage;
        private readonly int _displayedPageCount;
        private readonly Lazy<Page> _last;
        private readonly int _lastPage;
        private readonly Lazy<Page> _next;
        private readonly Lazy<Page> _previous;
        private readonly ITableUrlManager _urlManager;

        public Paginator(ITableUrlManager urlManager, int totalRecords, int pageSize, int displayedPageCount,
                         int currentPage)
        {
            _urlManager = urlManager;

            var last = (int) Math.Ceiling(totalRecords/(double) pageSize);
            _last = new Lazy<Page>(() => new Page(last, _urlManager.GetPagedUrl(last)));

            var previous = Math.Max(currentPage - 1, 1);
            _previous = new Lazy<Page>(() => new Page(previous, _urlManager.GetPagedUrl(previous)));

            var next = Math.Min(currentPage + 1, last);
            _next = new Lazy<Page>(() => new Page(next, _urlManager.GetPagedUrl(next)));

            _displayedPageCount = displayedPageCount;
            _currentPage = currentPage;
            _lastPage = last;
        }

        public Page First
        {
            get { return new Page(1, _urlManager.GetPagedUrl(1)); }
        }

        public Page Last
        {
            get { return _last.Value; }
        }

        public Page Previous
        {
            get { return _previous.Value; }
        }

        public Page Next
        {
            get { return _next.Value; }
        }

        public Page Current
        {
            get { return new Page(_currentPage, _urlManager.GetPagedUrl(_currentPage), true); }
        }

        public IEnumerable<Page> Pages
        {
            get
            {
                var displayedPages = Math.Min(_displayedPageCount, _lastPage);
                if (displayedPages != 0)
                {
                    var initialRange = Enumerable.Range(1, displayedPages).ToArray();
                    var offset = initialRange.Contains(_currentPage)
                                     ? 0
                                     : _currentPage > initialRange.Max()
                                           ? _currentPage - initialRange.Max()/2
                                           : _currentPage - initialRange.Min()/2;

                    foreach (var i in initialRange.Select(n => n + offset))
                    {
                        if (i <= _lastPage)
                        {
                            yield return new Page(i, _urlManager.GetPagedUrl(i), i == _currentPage);
                        }
                    }
                }
            }
        }
    }
}