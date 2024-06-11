using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Interfaces
{
    public interface ICategoriesService
    {

        Task<AddCategoryResDTO?> AddCategoryAsync(AddCategoryReqDTO category);
    }
}
