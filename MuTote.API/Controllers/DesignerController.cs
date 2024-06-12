using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;

namespace MuTote.API.Controllers
{
    [Route("api/designers")]
    [ApiController]
    public class DesignerController : Controller
    {
        private readonly IDesignerService _userService;

        public DesignerController(IDesignerService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get list of designers
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<DesignerResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesigners([FromQuery] PagingRequest pagingRequest, [FromQuery] DesignerRequest userRequest)
        {
            var rs = await _userService.GetDesigners(userRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get designer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesigner(int id)
        {
            var rs = await _userService.GetDesignerById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Update profile of designer
        /// </summary>
        /// <param name="userRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "designer")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDesigner([FromBody] UpdateDesignerRequest userRequest, int id)
        {
            var rs = await _userService.UpdateDesigner(id, userRequest);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Delete designer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet("{cusId:int}/blocked-user")]
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
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
            var rs = await _userService.Verification(phone, token);
            return Ok(rs);
        }
        /// <summary>
        /// Sign Up account of designer by phone
        /// </summary>
        /// <param name="designer"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDesigner([FromBody] CreateDesignerRequest designer)
        {
            var rs = await _userService.CreateDesigner(designer);
            return Ok(rs);
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authentication")]
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType(typeof(DesignerResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest resetPassword)
        {
            var rs = await _userService.UpdatePass(resetPassword);
            return Ok(rs);
        }
    }
}
