namespace TabProjectServer.Models.Domain
{
    public class Loan
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required Guid BookId { get; set; }
        public required DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public required User User { get; set; }
        public required Book Book { get; set; }
    }
}
