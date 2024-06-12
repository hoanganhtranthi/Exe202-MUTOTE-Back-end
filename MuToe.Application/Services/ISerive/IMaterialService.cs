

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;

namespace MuTote.Application.Services.ISerive
{
    public interface IMaterialService
    {
        Task<PagedResults<MaterialResponse>> GetMaterials(MaterialRequest request, PagingRequest paging);
        Task<MaterialResponse> GetMaterialById(int id);
        Task<MaterialResponse> DeleteMaterial(int id);
        Task<MaterialResponse> InsertMaterial(CreateMaterialRequest category);
    }
}
