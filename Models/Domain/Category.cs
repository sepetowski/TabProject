namespace TabProjectServer.Models.Domain
{
    public class Category
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }


        public  ICollection<Book> Books { get; set; }
    }
}
