namespace TabProjectServer.Models.DTO.Loan
{
    public class GetUserLoansResDTO
    {
        public required List<LoanDTO> Loans { get; set; }
        public required int Amount { get; set; }
    }
}
