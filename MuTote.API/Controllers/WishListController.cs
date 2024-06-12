using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Service.Services.ISerive;
using System.Net;

namespace MuTote.API.Controllers
{
    [Route("api/wishlists")]
    [ApiController]
    public class WishListController : Controller
    {
        private readonly IWishListService _wishlistService;
        public WishListController(IWishListService wishlistService)
        {
            _wishlistService = wishlistService;
        }
        /// <summary>
        /// Get list of wishlists
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="wishlistRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<WishListResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWishLists([FromQuery] PagingRequest pagingRequest, [FromQuery] WishListRequest wishlistRequest)
        {
            var rs = await _wishlistService.GetWishLists(wishlistRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get wishlist by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(WishListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWishList(int id)
        {
            var rs = await _wishlistService.GetWishListById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Update information of wishlist
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "customer")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(WishListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateWishList(int quantity, int id)
        {
            var rs = await _wishlistService.UpdateWishList(id,quantity);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Delete wishlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "customer")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(WishListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var rs = await _wishlistService.DeleteWishList(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Create wishlist
        /// </summary>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        [Authorize(Roles = "customer")]
        [HttpPost()]
        [ProducesResponseType(typeof(WishListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateWishList(CreateWishListRequest wishlist)
        {
            var rs = await _wishlistService.InsertWishList(wishlist);
            return Ok(rs);
        }
    }
}
