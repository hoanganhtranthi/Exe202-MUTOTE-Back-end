

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;

namespace MuTote.Application.Services.ISerive
{
    public interface ICustomerService
    {
        Task<PagedResults<CustomerResponse>> GetCustomers(CustomerRequest request, PagingRequest paging);
        Task<CustomerResponse> GetToUpdateStatus(int id);
        Task<string> Verification(string request, string token);
        Task<JWTResponse> Login(LoginRequest request);
        Task<JWTResponse> CreateCustomer(CreateCustomerRequest request);
        Task<CustomerResponse> GetCustomerById(int id);
        Task<CustomerResponse> UpdatePass(ResetPasswordRequest request);
        Task<CustomerResponse> UpdateCustomer(int customerId, UpdateCustomerRequest request);
    }
}
