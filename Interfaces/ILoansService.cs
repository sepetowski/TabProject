using TabProjectServer.Models.DTO.Loan;

namespace TabProjectServer.Interfaces
{
    public interface ILoansService
    {

        Task<CreateLoanResDTO?> CreateLoanAsync(CreateLoanReqDTO req);
        Task<ReturnLoanResDTO?> ReturnLoanAsync(ReturnLoanReqDTO req);
        Task<GetAllLoansResDTO?> GetAllLoansAsync();
        Task<GetUserLoansResDTO?> GetUserLoansAsync(Guid id);

    }
}
