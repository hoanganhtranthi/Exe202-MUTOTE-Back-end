

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;

namespace MuTote.Application.Services.ISerive
{
    public interface IDesignerService
    {
        Task<PagedResults<DesignerResponse>> GetDesigners(DesignerRequest request, PagingRequest paging);
        Task<DesignerResponse> GetToUpdateStatus(int id);
        Task<string> Verification(string request, string token);
        Task<DesignerResponse> Login(LoginRequest request);
        Task<DesignerResponse> CreateDesigner(CreateDesignerRequest request);
        Task<DesignerResponse> GetDesignerById(int id);
        Task<DesignerResponse> UpdatePass(ResetPasswordRequest request);
        Task<DesignerResponse> UpdateDesigner(int customerId, UpdateDesignerRequest request);
    }
}
