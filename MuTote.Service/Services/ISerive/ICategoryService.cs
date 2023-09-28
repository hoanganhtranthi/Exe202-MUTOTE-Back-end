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
    public interface ICategoryService
    {
        Task<PagedResults<CategoryResponse>> GetCategorys(CategoryRequest request, PagingRequest paging,CategoryChoice choice);
        Task<CategoryResponse> DeleteCategory(int id, CategoryChoice choice);
        Task<CategoryResponse> GetCategoryById(int id, CategoryChoice choice);
        Task<CategoryResponse> UpdateCategory(int id, CreateCategoryRequest request,CategoryChoice choice);
        Task<CategoryResponse> InsertCategory(CreateCategoryRequest category, CategoryChoice choice);
    }
}
