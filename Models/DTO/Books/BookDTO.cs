namespace TabProjectServer.Models.DTO.Books
{
    public class BookDTO
    {
        public required Guid Id { get; set; }
        public required Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string NumberOfPage { get; set; }
        public required int PublicationYear { get; set; }
    }
}
