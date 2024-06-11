namespace TabProjectServer.Models.DTO.Books
{
    public class GetAllBooksResDTO
    {
        public required List<BookWithAuthorAndCategoriesDTO> Books { get; set; }
        public required int Amount { get; set; }
    }
}
