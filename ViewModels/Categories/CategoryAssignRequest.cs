using Solution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.ViewModels.Categories
{
    public class CategoryAssignRequest
    {
        public int Id { set; get; }

        public List<SelectedItem> Categories { set; get; } = new List<SelectedItem>();
    }
}