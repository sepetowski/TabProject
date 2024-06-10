using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Auth;
using TabProjectServer.Models.DTO.Authors;

namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddAuthorReqDTO req)
        {
            try
            {
              var res = await  _authorsService.CreateAuthorAsync(req);
                return Ok(res);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            
    }
}
