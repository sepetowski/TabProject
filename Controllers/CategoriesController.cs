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



        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryReqDTO addCategoryReqDTO)
        {
            var res=await _categoriesService.AddCategoryAsync(addCategoryReqDTO);

            if (res == null) return BadRequest("Category already exist");

            return Ok(res);

        }
    }
}
