
namespace TabProjectServer.Models.DTO.Categories
{
    public class GetAllCategoriesResDTO
    {
        public required List<CategoryDTO> Categories { get; set; }
        public required int Amount { get; set; }
    }
}
