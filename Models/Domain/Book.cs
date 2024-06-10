namespace TabProjectServer.Models.Domain
{
    public class Book
    {
        public required Guid Id { get; set; }
        public required Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public required string NumberOfPage { get; set; }
        public required int PublicationYear { get; set; }


        public required Author Author { get; set; }
        public required ICollection<Category> Categories { get; set; }



    }
}
