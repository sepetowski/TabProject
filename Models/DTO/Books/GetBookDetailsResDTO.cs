using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Models.DTO.Books
{
    public class GetBookDetailsResDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string BookDescripton { get; set; }
        public required int NumberOfPage { get; set; }
        public required DateTime PublicationDate { get; set; }

        public int? availableCopies {  get; set; }
        public string? ImageUrl { get; set; } = null;
        public required bool isAvaible { get; set; }

        public required Guid AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public required string AuthorSurname { get; set; }
        public string? AuthorDescription { get; set; }
        public DateTime? AuthorDateOfBirth { get; set; }

        public required List<CategoryDTO> Categories { get; set; }
    }
}
