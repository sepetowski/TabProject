using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Categories
{
    public class AddCategoryReqDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
