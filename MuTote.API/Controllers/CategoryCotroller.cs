using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Services.ISerive;
using System.Data;


namespace MuTote.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        /// Get list of categories
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="categoryRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategorys([FromQuery] PagingRequest pagingRequest, [FromQuery] CategoryRequest categoryRequest, Service.Helpers.Enum.CategoryChoice choice)
        {
            var rs = await _categoryService.GetCategorys(categoryRequest, pagingRequest,choice);
            return Ok(rs);
        }
        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponse>> GetCategory(int id, Service.Helpers.Enum.CategoryChoice choice)
        {
            var rs = await _categoryService.GetCategoryById(id,choice);
            return Ok(rs);
        }
        /// <summary>
        /// Update information of category
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
       // [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory([FromBody] CreateCategoryRequest categoryRequest, int id, Service.Helpers.Enum.CategoryChoice choice)
        {
            var rs = await _categoryService.UpdateCategory(id, categoryRequest,choice);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryResponse>> DeleteCategory(int id, Service.Helpers.Enum.CategoryChoice choice)
        {
            var rs = await _categoryService.DeleteCategory(id, choice);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpPost()]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest category, Service.Helpers.Enum.CategoryChoice choice)
        {
            var rs = await _categoryService.InsertCategory(category,choice);
            return Ok(rs);
        }
    }
}
