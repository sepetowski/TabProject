using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Loan;


namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {

        private readonly ILoansService _loansService;

        public LoansController(ILoansService loansService)
        {
            _loansService = loansService;
        }


        [HttpGet()]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _loansService.GetAllLoansAsync();

            return Ok(res);
        }


        [HttpGet("GetUserLoans/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserLoans([FromRoute] Guid id)
        {
            var res = await _loansService.GetUserLoansAsync(id);

            if (res == null) return BadRequest("User not found");
            return Ok(res);
        }


        [HttpPost("CreateLoan")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanReqDTO req)
        {
            try
            {

            var res = await _loansService.CreateLoanAsync(req);

            return Ok(res);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("ReturnLoan")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ReturnLoan([FromBody] ReturnLoanReqDTO req)
        {
            try
            {

                 var res = await _loansService.ReturnLoanAsync(req);
                 return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
