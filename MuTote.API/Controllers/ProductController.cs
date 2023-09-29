using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Services.ISerive;

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
        public async Task<ActionResult<List<ProductResponse>>> GetProducts([FromQuery] PagingRequest pagingRequest, [FromQuery] ProductRequest productRequest)
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
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            var rs = await _productService.GetProductById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Get best seller products
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("best-seller-products")]
        public async Task<ActionResult<ProductResponse>> GetProductsBestSeller([FromQuery] PagingRequest pagingRequest)
        {
            var rs = await _productService.GetBestSellerProduct(pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Update product's unit in stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponse>> DeleteProduct(int id, int unitInStock)
        {
            var rs = await _productService.UpdateProduct(id,unitInStock);
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
        public async Task<ActionResult<ProductResponse>> CreateProduct(CreateProductRequest product)
        {
            var rs = await _productService.InsertProduct(product);
            return Ok(rs);
        }
    }
}
