using Solution.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAll();

        Task<CategoryVM> GetById(int id);
    }
}