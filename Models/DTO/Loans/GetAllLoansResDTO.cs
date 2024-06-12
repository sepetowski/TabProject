
namespace TabProjectServer.Models.DTO.Loan
{
    public class GetAllLoansResDTO
    {
        public required List<LoanDTO> Loans { get; set; }
        public required int Amount { get; set; }
    }
}
