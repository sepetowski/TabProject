namespace TabProjectServer.Models.Domain
{
    public class Book
    {
        public required Guid Id { get; set; }
        public required Guid AuthorId { get; set; }
        public required string BookDescripton { get; set; }
        public required string Title { get; set; }
        public required int NumberOfPage { get; set; }
        public required DateTime PublicationDate { get; set; }

        public required int AvailableCopies { get; set; }

        public string? ImageUrl { get; set; } = null;

        public required Author Author { get; set; }
        public ICollection<Category> Categories { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();



    }
}
