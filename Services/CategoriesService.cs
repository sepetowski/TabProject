using Microsoft.EntityFrameworkCore;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Services
{
    public class CategoriesService : ICategoriesService
    {

        private readonly DataContext _context;

        public CategoriesService(DataContext context) {
            _context = context;
        }
        public async Task<AddCategoryResDTO?> AddCategoryAsync(AddCategoryReqDTO category)
        {

            var existingCategory=  await _context.Categories.FirstOrDefaultAsync(c=> c.Name.ToLower()== category.Name.ToLower());

            if (existingCategory != null) return null;


            var newCategory = new Category
            {
                Id = new Guid(),
                Name = category.Name,
                Books = []
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            var res = new AddCategoryResDTO
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
            };

            return res;
        }
    }
}
