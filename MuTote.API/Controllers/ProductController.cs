using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Get list of products
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts([FromQuery] PagingRequest pagingRequest, [FromQuery] ProductRequest productRequest)
        {
            var rs = await _productService.GetProducts(productRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var rs = await _productService.GetProductById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Update product's unit in stock and status ( NewProduct=1, Avaliable = 2, OutOfStock = 0)
        /// </summary> 
        /// <param name="id"></param>
        /// /// <param name="unitInStock"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct(int id, int? unitInStock, ProductStatusEnum? status)
        {
            var rs = await _productService.UpdateProduct(id,unitInStock,status);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost()]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct(CreateProductRequest product)
        {
            var rs = await _productService.InsertProduct(product);
            return Ok(rs);
        }
    }
}
