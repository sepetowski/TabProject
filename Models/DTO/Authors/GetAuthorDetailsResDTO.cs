using TabProjectServer.Models.DTO.Books;

namespace TabProjectServer.Models.DTO.Authors
{
    public class GetAuthorDetailsResDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public required List<BookWithCategoriesDTO> Books{ get; set; } = new();
        public required int Amount { get; set; }
    }
}
