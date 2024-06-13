
using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Models.DTO.Books
{
    public class UpdateBookResDTO
    {

        public required Guid Id { get; set; }
        public required string Title { get; set; }

        public required string BookDescripton { get; set; }

        public required int NumberOfPage { get; set; }
      
        public required DateTime PublicationDate { get; set; }

        public string? ImageUrl { get; set; }

        public required int AvailableCopies { get; set; }

        public required List<CategoryDTO> Categories { get; set; }
    }
}
