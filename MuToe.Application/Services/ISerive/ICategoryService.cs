

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.Application.Services.ISerive
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetCategorys(CategoryChoice choice);
        Task<CategoryResponse> DeleteCategory(int id, CategoryChoice choice);
        Task<CategoryResponse> GetCategoryById(int id, CategoryChoice choice);
        Task<CategoryResponse> UpdateCategory(int id, CreateCategoryRequest request,CategoryChoice choice);
        Task<CategoryResponse> InsertCategory(CreateCategoryRequest category, CategoryChoice choice);
    }
}
