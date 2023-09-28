using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.Services.ISerive
{
    public interface ICustomerService
    {
        Task<PagedResults<CustomerResponse>> GetCustomers(CustomerRequest request, PagingRequest paging);
        Task<CustomerResponse> GetToUpdateStatus(int id);
        Task<string> Verification(string request, string token);
        Task<CustomerResponse> Login(LoginRequest request);
        Task<CustomerResponse> LoginByGoogle(string googleId);
        Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request);
        Task<CustomerResponse> GetCustomerById(int id);
        Task<string> GetJwt(int accountId);
        Task<CustomerResponse> UpdatePass(ResetPasswordRequest request);
        Task<CustomerResponse> UpdateCustomer(int customerId, UpdateCustomerRequest request);
    }
}
