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

        public async Task<DeleteCategoryResDTO?> DeleteCategoryAsync(Guid id)
        {
            var exisitngCategory= await _context.Categories.FindAsync(id);

            if (exisitngCategory == null) return null;

           _context.Categories.Remove(exisitngCategory);

            await _context.SaveChangesAsync();

            return new DeleteCategoryResDTO { Id = exisitngCategory.Id };
        }

        public async Task<GetAllCategoriesResDTO> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();


            return new GetAllCategoriesResDTO
            {
                Categories = categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList(),
                Amount = categories.Count
            };
        }

        public async Task<UpdateCategoryResDTO?> UpdateCategoryAsync(Guid id,UpdateCategoryReqDTO category)
        {
         

            var exisitngCategory = await _context.Categories.FindAsync(id);


            if (exisitngCategory == null) throw new Exception("Category not found");


            if (exisitngCategory.Name.ToLower() == category.Name.ToLower())
                throw new Exception("Name must be diffrent form current one");


            var checkName = await _context.Categories
                             .FirstOrDefaultAsync(c => c.Name == category.Name && c.Id != id);

            if (checkName != null)
                throw new Exception("Category with this name already exists");

            exisitngCategory.Name=category.Name;

            await _context.SaveChangesAsync();

            return new UpdateCategoryResDTO
            {
                Id = exisitngCategory.Id,
                Name = exisitngCategory.Name
            };


        }
    }
}
