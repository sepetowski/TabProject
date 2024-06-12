using System.ComponentModel.DataAnnotations;

namespace TabProjectServer.Models.DTO.Loan
{
    public class ReturnLoanReqDTO
    {
        [Required]
        public Guid Id { get; set; }
    }
}
