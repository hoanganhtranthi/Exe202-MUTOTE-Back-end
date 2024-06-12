

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.Application.Services.ISerive
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
