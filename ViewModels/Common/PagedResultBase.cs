using System;

namespace Solution.ViewModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ToTalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)ToTalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}