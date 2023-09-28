using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuTote.Service.Helpers.Enum;

namespace MuTote.Service.Services.ISerive
{
    public interface IMaterialService
    {
        Task<PagedResults<MaterialResponse>> GetMaterials(MaterialRequest request, PagingRequest paging);
        Task<MaterialResponse> GetMaterialById(int id);
        Task<MaterialResponse> DeleteMaterial(int id);
        Task<MaterialResponse> InsertMaterial(CreateMaterialRequest category);
    }
}
