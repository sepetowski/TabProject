namespace TabProjectServer.Models.DTO.Loan
{
    public class ReturnLoanResDTO
    {
        public required Guid Id { get; set; }
        public required DateTime ReturnDate { get; set; }
    }
}
