using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Categories
{
    public class UpdateCategoryReqDTO
    {
      
        [Required]
        public string Name { get; set; }
    }
}
