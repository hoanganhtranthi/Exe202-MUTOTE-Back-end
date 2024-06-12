using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;

namespace MuTote.API.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        /// <summary>
        /// Get list of materials
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="materialRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<MaterialResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMaterials([FromQuery] PagingRequest pagingRequest, [FromQuery] MaterialRequest materialRequest)
        {
            var rs = await _materialService.GetMaterials(materialRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get material by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMaterial(int id)
        {
            var rs = await _materialService.GetMaterialById(id);
            return Ok(rs);
        }
       
        /// <summary>
        /// Delete material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var rs = await _materialService.DeleteMaterial(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        /// <summary>
        /// Create material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin" + "," + "designer")]
        [HttpPost()]
        [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMaterial(CreateMaterialRequest material)
        {
            var rs = await _materialService.InsertMaterial(material);
            return Ok(rs);
        }
    }
}
