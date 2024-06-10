namespace TabProjectServer.Models.DTO.Auth
{
    public class UserRegisterResDTO
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}
