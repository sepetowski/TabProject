namespace TabProjectServer.Models.DTO.Auth
{
    public class RefreshTokenResDTO
    {
        public required string Token { get; set; }
        public required DateTime TokenExpires { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpires { get; set; }
    }
}
