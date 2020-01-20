using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class PaginatedList<T> : List<T>
    {
        private PaginatedList(
            List<T> items, int count, int pageIndex, int pageSize, int countOfPageIndexesToDisplay)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            SetPageIndexesToDisplay(pageIndex, countOfPageIndexesToDisplay); AddRange(items);
        }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);

        public List<PageIndex> PageIndexesToDisplay { get; private set; }

        public static PaginatedList<T> Create(
            IEnumerable<T> source, int pageIndex, int pageSize, int countOfPageIndexesToDisplay = 3)
        {
            var list = source.ToList();
            var count = list.Count;
            var items = list.Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize, countOfPageIndexesToDisplay);
        }

        public void SetPageIndexesToDisplay(int pageIndex, int countOfPageIndexesToDisplay)
        {
            PageIndexesToDisplay = new List<PageIndex>();

            if (pageIndex > TotalPages - countOfPageIndexesToDisplay + Math.Floor(countOfPageIndexesToDisplay / 2.0m))
            {
                for (var i = Math.Max(TotalPages - countOfPageIndexesToDisplay + 1, 1); i <= TotalPages; i++)
                {
                    PageIndexesToDisplay.Add(new PageIndex(i, i == pageIndex));
                }
            }
            else if (pageIndex < (countOfPageIndexesToDisplay + 1) - Math.Floor(countOfPageIndexesToDisplay / 2.0m))
            {
                for (var i = 1; i <= Math.Min(countOfPageIndexesToDisplay, TotalPages); i++)
                {
                    PageIndexesToDisplay.Add(new PageIndex(i, i == pageIndex));
                }
            }
            else
            {
                var startIndex = pageIndex - (int)Math.Floor(countOfPageIndexesToDisplay / 2.0m);
                for (var i = startIndex; i <= startIndex + countOfPageIndexesToDisplay - 1; i++)
                {
                    PageIndexesToDisplay.Add(new PageIndex(i, i == pageIndex));
                }
            }
        }
    }
}
