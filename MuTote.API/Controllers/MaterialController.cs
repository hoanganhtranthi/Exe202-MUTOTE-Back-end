using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Services.ISerive;
using System.Data;

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
        public async Task<ActionResult<List<MaterialResponse>>> GetMaterials([FromQuery] PagingRequest pagingRequest, [FromQuery] MaterialRequest materialRequest)
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
        public async Task<ActionResult<MaterialResponse>> GetMaterial(int id)
        {
            var rs = await _materialService.GetMaterialById(id);
            return Ok(rs);
        }
       
        /// <summary>
        /// Delete material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MaterialResponse>> DeleteMaterial(int id)
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
        public async Task<ActionResult<MaterialResponse>> CreateMaterial(CreateMaterialRequest material)
        {
            var rs = await _materialService.InsertMaterial(material);
            return Ok(rs);
        }
    }
}
