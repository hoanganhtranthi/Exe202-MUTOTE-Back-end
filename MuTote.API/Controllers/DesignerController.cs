using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Services.ISerive;

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
        public async Task<ActionResult<List<DesignerResponse>>> GetDesigners([FromQuery] PagingRequest pagingRequest, [FromQuery] DesignerRequest userRequest)
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
        public async Task<ActionResult<DesignerResponse>> GetDesigner(int id)
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
        public async Task<ActionResult<DesignerResponse>> UpdateDesigner([FromBody] UpdateDesignerRequest userRequest, int id)
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
        public async Task<ActionResult<DesignerResponse>> GetToUpdateStatus(int cusId)
        {
            var rs = await _userService.GetToUpdateStatus(cusId);
            return Ok(rs);
        }
        /// <summary>
        /// Send OTP 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="phone"></param>
        /// <param name="googleId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verification")]
        public async Task<ActionResult<string>> Verification([FromQuery] string phone, [FromQuery] string? token)
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
        public async Task<ActionResult<DesignerResponse>> CreateDesigner([FromBody] CreateDesignerRequest designer)
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
        public async Task<ActionResult<DesignerResponse>> Login([FromBody] LoginRequest model)
        {
            var rs = await _userService.Login(model);
            return Ok(rs);
        }
        /// <summary>
        /// Sign Up account of designer by googleId
        /// </summary>
        /// <param name="googleId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("google-authentication")]
        public async Task<ActionResult<DesignerResponse>> LoginGoogle([FromQuery] string googleId)
        {
            var rs = await _userService.LoginByGoogle(googleId);
            return Ok(rs);
        }
        /// <summary>
        /// Reset password when forgot password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgotten-password")]
        public async Task<ActionResult<DesignerResponse>> ResetPassword([FromQuery] ResetPasswordRequest resetPassword)
        {
            var rs = await _userService.UpdatePass(resetPassword);
            return Ok(rs);
        }
        /// <summary>
        /// Get JWT of account
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpGet("get-jwt/{id}")]
        public async Task<ActionResult<string>> GetJwt(int id)
        {
            var rs = await _userService.GetJwt(id);
            return Ok(rs);
        }
    }
}
