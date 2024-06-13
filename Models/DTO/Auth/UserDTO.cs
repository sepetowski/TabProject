namespace TabProjectServer.Models.DTO.Auth
{
    public class UserDTO
    {
        public required Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


    }
}
