using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.Services.ISerive
{
    public interface IProductService
    {
        Task<PagedResults<ProductResponse>> GetProducts(ProductRequest request, PagingRequest paging);
        Task<ProductResponse> GetProductById(int id);
       Task<ProductResponse> UpdateProduct(int id, int unitInStock);
        Task<ProductResponse> InsertProduct(CreateProductRequest product);
        Task<PagedResults<ProductResponse>> GetBestSellerProduct(PagingRequest paging);
       
    }
}
