using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Authors;


namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }


        [HttpPost()]
        [Authorize(Policy = "AdminOnly")]
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

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var res= await _authorsService.GetAllAuthorsAsync();

            return Ok(res);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetAuthorDetails([FromRoute] Guid id)
        {
            var res= await _authorsService.GetAuthorDetailsAsync(id);

            if (res == null) return BadRequest("Author not found");

            return Ok(res);
        }



        [HttpPut()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateAuthorReqDTO req)
        {
            var res = await _authorsService.UpdateAuthorAsync(id, req);

            if (res == null) return BadRequest("Author not found");

            return Ok(res);
        }

        [HttpDelete()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var res= await _authorsService.DeleteAuthorAsync(id);

            if (res == null) return BadRequest("Author not found");

            return Ok(res);
        }
    }
}
