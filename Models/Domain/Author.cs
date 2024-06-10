namespace TabProjectServer.Models.Domain
{
    public class Author
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Description { get; set; }
        public DateTime? BateOfBirth { get; set; }


        public required ICollection<Book> Books { get; set; }
    }
}
