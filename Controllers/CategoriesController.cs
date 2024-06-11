using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Categories;


namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService) {
            _categoriesService = categoriesService;
        }



        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoriesService.GetAllCategoriesAsync();

            return Ok(res);
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryReqDTO addCategoryReqDTO)
        {
            var res=await _categoriesService.AddCategoryAsync(addCategoryReqDTO);

            if (res == null) return BadRequest("Category already exist");

            return Ok(res);

        }

        [HttpPut()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryReqDTO req)
        {
            try
            {

                var res = await _categoriesService.UpdateCategoryAsync(id, req);

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete()]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var res = await _categoriesService.DeleteCategoryAsync(id);

            if (res == null) return BadRequest("Category not found");

            return Ok(res);
        }
    }
}
