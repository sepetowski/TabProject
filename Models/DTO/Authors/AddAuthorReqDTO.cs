using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Authors
{
    public class AddAuthorReqDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
