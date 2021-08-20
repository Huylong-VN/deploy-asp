using Solution.ViewModels.Common;
using System;
using System.Collections.Generic;

namespace Solution.ViewModels.Roles
{
    public class RoleAssignRequest
    {
        public Guid userId { get; set; }

        public List<SelectedItem> Roles { get; set; } = new List<SelectedItem>();
    }
}