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
    public interface IWishListService
    {
        Task<PagedResults<WishListResponse>> GetWishLists(WishListRequest request, PagingRequest paging);
        Task<WishListResponse> DeleteWishList(int id);
        Task<WishListResponse> GetWishListById(int id);
        Task<WishListResponse> UpdateWishList(int id, int quantity);
        Task<WishListResponse> InsertWishList(CreateWishListRequest wishList);
    }
}
