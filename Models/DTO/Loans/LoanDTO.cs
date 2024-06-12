namespace TabProjectServer.Models.DTO.Loan
{
    public class LoanDTO
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Username { get; set; }
        public required Guid BookId { get; set; }
        public required string BookTitle { get; set; }
        public required string BookAuthor { get; set; }
        public required DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }



    }
}
