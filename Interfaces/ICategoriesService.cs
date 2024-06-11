using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Interfaces
{
    public interface ICategoriesService
    {

        Task<AddCategoryResDTO?> AddCategoryAsync(AddCategoryReqDTO category);
        Task<DeleteCategoryResDTO?> DeleteCategoryAsync(Guid id);
        Task<GetAllCategoriesResDTO> GetAllCategoriesAsync();
        Task<UpdateCategoryResDTO?> UpdateCategoryAsync(Guid id, UpdateCategoryReqDTO category);
    }
}
