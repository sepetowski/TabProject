using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Authors;
using TabProjectServer.Models.DTO.Books;
using TabProjectServer.Models.DTO.Categories;
using TabProjectServer.Services;

namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
           _booksService = booksService;
        }


        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var res = await _booksService.GetAllBooksAsync();

            return Ok(res);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetBookDetails([FromRoute] Guid id)
        {
            var res = await _booksService.GetBookDetailsAsync(id);

            if (res == null) return BadRequest("Book not found");

            return Ok(res);
        }


        [HttpPut()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateBookReqDTO req)
        {
            try
            {

            var res = await _booksService.UpdateBookAsync(id, req);

                return Ok(res);

            }catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddBook([FromBody] AddBookReqDTO req)
        {
            try
            {
                var res= await  _booksService.AddBookAsync(req);

                return Ok(res); 
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var res = await _booksService.DeleteBookResAsync(id);

            if (res == null) return BadRequest("Book not found");

            return Ok(res);
        }
    }
}
