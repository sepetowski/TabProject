using TabProjectServer.Models.DTO.Categories;

namespace TabProjectServer.Models.DTO.Books
{
    public class BookWithAuthorAndCategoriesDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }

        public string? ImageUrl { get; set; }= null;

        public required Guid AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public required string AuthorSurname { get; set; }

        public required bool isAvaible { get; set; }


        public required List<CategoryDTO> Categories { get; set; }


    }
}
