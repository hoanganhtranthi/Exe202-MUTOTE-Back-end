using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.Services.ISerive
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
