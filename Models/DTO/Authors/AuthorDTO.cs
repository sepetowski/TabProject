namespace TabProjectServer.Models.DTO.Authors
{
    public class AuthorDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
