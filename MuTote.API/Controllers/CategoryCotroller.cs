using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;
using static MuTote.Domain.Enums.Enum;

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
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategorys(CategoryChoice choice)
        {
            var rs = await _categoryService.GetCategorys(choice);
            return Ok(rs);
        }
        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategory(int id,CategoryChoice choice)
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
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory([FromBody] CreateCategoryRequest categoryRequest, int id, CategoryChoice choice)
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
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCategory(int id, CategoryChoice choice)
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
        [Authorize(Roles = "admin")]
        [HttpPost()]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest category,CategoryChoice choice)
        {
            var rs = await _categoryService.InsertCategory(category,choice);
            return Ok(rs);
        }
    }
}
