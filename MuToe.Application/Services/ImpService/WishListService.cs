using AutoMapper;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.GlobalExceptionHandling.Exceptions;
using MuTote.Application.Helpers;
using MuTote.Application.Services.ISerive;
using MuTote.Application.UnitOfWork;
using MuTote.Application.Utilities;
using MuTote.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using MuTote.Service.Services.ISerive;
using System.Net;


namespace MuTote.Application.Services.ImpService
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WishListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<WishListResponse> DeleteWishList(int id)
        {
            try
            {               
                    var wishList = _unitOfWork.Repository<WishList>().GetAll().Include(c=>c.Product).Where(c => c.Id == id).SingleOrDefault();
                    if (wishList == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, "Not found wish list with id", "");
                    }
                    _unitOfWork.Repository<WishList>().Delete(wishList);
                    await _unitOfWork.CommitAsync();
                    var rs=_mapper.Map<WishList, WishListResponse>(wishList);
                rs.Price = rs.Quantity * wishList.Product.Price;
                rs.ProductName = wishList.Product.Name;
                rs.ProductImg = wishList.Product.Img;
                return rs;
               }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete Wish List Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<WishListResponse> GetWishListById(int id)
        {
            try
            {
                if (id <= 0) throw new CrudException(HttpStatusCode.BadRequest, "Id Wish List Invalid", "");               
                    var response = _unitOfWork.Repository<WishList>().GetAll().Include(c => c.Product).Where(c => c.Id == id).SingleOrDefault();

                if (response == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, $"Not found wish list with id{id.ToString()}", "");
                    }

                var rs = _mapper.Map<WishList, WishListResponse>(response);
                rs.Price = rs.Quantity * response.Product.Price;
                rs.ProductName = response.Product.Name;
                rs.ProductImg = response.Product.Img;
                return rs;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Wish List By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<PagedResults<WishListResponse>> GetWishLists(WishListRequest request, PagingRequest paging)
        {
            try
            {
                var filter = _mapper.Map<WishListResponse>(request);
                IList<WishListResponse> wishList = new List<WishListResponse>();
                    wishList = _unitOfWork.Repository<WishList>().GetAll().Select(x=>new WishListResponse
                    {
                        Id = x.Id,
                        CustomerId=x.CustomerId,
                        ProductId=x.ProductId,
                        ProductImg=_unitOfWork.Repository<Product>().GetAll().Where(a=>a.Id==x.ProductId).SingleOrDefault().Img,
                        ProductName= _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Name,
                        Quantity=x.Quantity,
                        Price= (_unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Price)*x.Quantity

                    }) .DynamicFilter(filter) .ToList();   
                var sort = PageHelper<WishListResponse>.Sorting(paging.SortType, wishList, paging.ColName);
                var result = PageHelper<WishListResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get wish list list error!!!!!", ex.Message);
            }
        }

        public async Task<WishListResponse> InsertWishList(CreateWishListRequest wishList)
        {
            try
            {
                    var wishListRequest = await _unitOfWork.Repository<WishList>().GetAsync(u => u.CustomerId==wishList.CustomerId && u.ProductId==wishList.ProductId);
                    if (wishList == null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Wish List Invalid!!!", "");
                    }
                    if (wishListRequest != null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "You has already insert this product to wish list !!!", "");
                    }

                    var response = _mapper.Map<CreateWishListRequest, WishList>(wishList);
                    await _unitOfWork.Repository<WishList>().CreateAsync(response);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<WishListResponse>(response);
                }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Insert Wish List Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<WishListResponse> UpdateWishList(int id, int quantity)
        {
            try
            {
                var wishList = _unitOfWork.Repository<WishList>().GetAll().Include(c => c.Product).Where(c => c.Id == id).SingleOrDefault();
                if (wishList == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found wish list with id", "");
                }
                wishList.Quantity = quantity;
                _unitOfWork.Repository<WishList>().Update(wishList,id);
                await _unitOfWork.CommitAsync();
                var rs = _mapper.Map<WishList, WishListResponse>(wishList);
                rs.Price = rs.Quantity * wishList.Product.Price;
                rs.ProductName = wishList.Product.Name;
                rs.ProductImg = wishList.Product.Img;
                return rs;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update Wish List Error!!!", ex.InnerException?.Message);
            }
        }
    }
}
