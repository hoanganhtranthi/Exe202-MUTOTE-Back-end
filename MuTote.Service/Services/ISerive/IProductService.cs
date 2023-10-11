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
    public interface IProductService
    {
        Task<PagedResults<ProductResponse>> GetProducts(ProductRequest request, PagingRequest paging);
        Task<ProductResponse> GetProductById(int id);
       Task<ProductResponse> UpdateProduct(int id, int? unitInStock, ProductStatusEnum? status);
        Task<ProductResponse> InsertProduct(CreateProductRequest product);
        Task<List<ProductResponse>> GetBestSellerProduct();
        Task<PagedResults<ProductResponse>> GetProductFilterByPrice(PagingRequest paging, decimal minPrice, decimal maxPrice, List<ProductResponse> listProduct);
    }
}
