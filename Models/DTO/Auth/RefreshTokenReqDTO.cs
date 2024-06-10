using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Auth
{
    public class RefreshTokenReqDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
