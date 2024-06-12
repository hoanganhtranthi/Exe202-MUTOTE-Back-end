

using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;

namespace MuTote.Service.Services.ISerive
{
    public interface IWishListService
    {
        Task<PagedResults<WishListResponse>> GetWishLists(WishListRequest request, PagingRequest paging);
        Task<WishListResponse> DeleteWishList(int id);
        Task<WishListResponse> GetWishListById(int id);
        Task<WishListResponse> UpdateWishList(int id, int quantity);
        Task<WishListResponse> InsertWishList(CreateWishListRequest wishList);
    }
}
