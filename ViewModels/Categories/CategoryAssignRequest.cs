using Solution.ViewModels.Common;
using System.Collections.Generic;

namespace Solution.ViewModels.Categories
{
    public class CategoryAssignRequest
    {
        public int Id { set; get; }

        public List<SelectedItem> Categories { set; get; } = new List<SelectedItem>();
    }
}