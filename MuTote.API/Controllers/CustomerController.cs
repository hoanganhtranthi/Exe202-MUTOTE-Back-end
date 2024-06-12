
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;

namespace MuTote.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _userService;

        public CustomerController(ICustomerService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get list of customers
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<CustomerResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomers([FromQuery] PagingRequest pagingRequest, [FromQuery] CustomerRequest userRequest)
        {
            var rs = await _userService.GetCustomers(userRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var rs = await _userService.GetCustomerById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Update profile of customer
        /// </summary>
        /// <param name="userRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "customer")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CustomerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerRequest userRequest, int id)
        {
            var rs = await _userService.UpdateCustomer(id, userRequest);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet("{cusId:int}/blocked-user")]
        [ProducesResponseType(typeof(CustomerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetToUpdateStatus(int cusId)
        {
            var rs = await _userService.GetToUpdateStatus(cusId);
            return Ok(rs);
        }
        /// <summary>
        /// Send OTP 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verification")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Verification([FromQuery] string phone, [FromQuery] string? token)
        {
            var rs = await _userService.Verification(phone,token);
            return Ok(rs);
        }
        /// <summary>
        /// Sign Up account of customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(typeof(JWTResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>CreateCustomer([FromBody] CreateCustomerRequest customer)
        {
            var rs = await _userService.CreateCustomer(customer);
            return Ok(rs);
        }
        /// <summary>
        /// Login by phone and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authentication")]
        [ProducesResponseType(typeof(JWTResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var rs = await _userService.Login(model);
            return Ok(rs);
        }
        /// <summary>
        /// Reset password when forgot password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgotten-password")]
        [ProducesResponseType(typeof(CustomerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest resetPassword)
        {
            var rs = await _userService.UpdatePass(resetPassword);
            return Ok(rs);
        }
    }
}
