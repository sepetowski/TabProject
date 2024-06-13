using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Auth
{
    public class UpdateUserReqDTO
    {

        [Required]
        public  string Username { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
