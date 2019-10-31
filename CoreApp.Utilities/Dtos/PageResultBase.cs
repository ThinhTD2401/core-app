using System;

namespace CoreApp.Utilities.Dtos
{
    public class PageResultBase
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int PageCount
        {
            get
            {
                double pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set => PageCount = value;
        }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);

    }
}
