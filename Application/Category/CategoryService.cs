using Microsoft.EntityFrameworkCore;
using Solution.Data.EF;
using Solution.ViewModels.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Application.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly SolutionDbContext _context;

        public CategoryService(SolutionDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryVM>> GetAll()
        {
            return await _context.Categories.Select(x => new CategoryVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<CategoryVM> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryVm = new CategoryVM()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return categoryVm;
        }
    }
}