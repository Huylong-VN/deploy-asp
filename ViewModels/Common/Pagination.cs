using System.Collections.Generic;

namespace Solution.ViewModels.Common
{
    public class Pagination<T> : PaginationBase
    {
        public List<T> Items { get; set; }
    }
}