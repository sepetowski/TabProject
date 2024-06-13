namespace TabProjectServer.Models.DTO.Auth
{
    public class UpdateUserResDTO
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
