using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using MuTote.Data.Enities;
using MuTote.Data.UnitOfWork;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Exceptions;
using MuTote.Service.Helpers;
using MuTote.Service.Services.ISerive;
using MuTote.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using static MuTote.Service.Helpers.Enum;

namespace MuTote.Service.Services.ImpService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ProductResponse>> GetBestSellerProduct()
        {
            try
            {
                var response = await _unitOfWork.Repository<OrderDetail>().GetAll().ToListAsync();
                var result = new List<OrderFilterRequest>();
                foreach (var order in response)
                {
                    var count = new OrderFilterRequest();
                    var cam = result.Where(a => a.ProductId==order.ProductId).SingleOrDefault();
                    if (cam != null) continue;
                    if (order.ProductId == null) continue;
                    count.ProductId = (int)order.ProductId;
                    count.Count = response.Where(a => a.ProductId == count.ProductId).ToList().Distinct().Count();
                    result.Add(count);
                }
                var orders = result.OrderByDescending(x => x.Count).Take(10);
                foreach (var ord in orders)
                {
                   var rs =_unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == ord.ProductId).SingleOrDefault();
                    if(rs != null)
                    {
                        rs.IsBestSeller = true;
                        _unitOfWork.Repository<Product>().Update(rs, ord.ProductId);
                        await _unitOfWork.CommitAsync();
                    }
                }
                var res= _unitOfWork.Repository<Product>().GetAll().Include(c => c.CategoryProduct)
                                         .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                                         .ToList();
                    return res;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Best Seller Product Error !!!", ex.InnerException?.Message);
            }
        }
       public async Task<PagedResults<ProductResponse>> GetProductFilterByPrice(PagingRequest paging, decimal minPrice, decimal maxPrice, List<ProductResponse> listProduct)
        {
            try
            {
                var list = listProduct.Where(a => a.Price >= minPrice && a.Price <= maxPrice).ToList();
                var sort = PageHelper<ProductResponse>.Sorting(paging.SortType, list, paging.ColName);
                var listPro = PageHelper<ProductResponse>.Paging(sort, paging.Page, paging.PageSize);
                return listPro;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get list Product filter by price  Error !!!", ex.InnerException?.Message);
            }
        }
        public async Task<ProductResponse> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Product Invalid", "");
                }
                var response = _unitOfWork.Repository<Product>().GetAll().Include(c => c.CategoryProduct).Where(u => u.Id == id).SingleOrDefault();

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found product with id {id.ToString()}", "");
                }

                return _mapper.Map<ProductResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Product By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<PagedResults<ProductResponse>> GetProducts(ProductRequest request, PagingRequest paging)
        {
            try
            {
                
                var filter = _mapper.Map<ProductResponse>(request);
                if (request.IsBestSeller ==false) filter.IsBestSeller = null;
                var products = _unitOfWork.Repository<Product>().GetAll().Include(c => c.CategoryProduct)
                                         .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).AsQueryable().DynamicFilter(filter)
                                         .ToList();
                if (request.maxPrice != null && request.minPrice != null) return GetProductFilterByPrice( paging, (decimal)request.minPrice, (decimal)request.maxPrice,products).Result;
               
                var sort = PageHelper<ProductResponse>.Sorting(paging.SortType, products, paging.ColName);
                var result = PageHelper<ProductResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }

            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product list error!!!!!", ex.Message);
            }
        }

        public async Task<ProductResponse> InsertProduct(CreateProductRequest product)
        {
            try
            {
                var productRequest = _unitOfWork.Repository<Product>().GetAll().Include(c => c.CategoryProduct).Where(u => u.Name == product.Name).SingleOrDefault();
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Product Invalid!!!", "");
                }
                if (productRequest != null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Product has already insert !!!", "");
                }

                var response = _mapper.Map<CreateProductRequest, Product>(product);
                response.Status = (int)ProductStatusEnum.NewProduct;
                response.IsBestSeller = false;
                await _unitOfWork.Repository<Product>().CreateAsync(response);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ProductResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Insert Product Error!!!", ex.InnerException?.Message);
            }
        }

       public async Task<ProductResponse> UpdateProduct(int id, int? unitInStock, ProductStatusEnum? status)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Product Invalid", "");
                }
                var response = _unitOfWork.Repository<Product>().GetAll().Include(c => c.CategoryProduct).Where(u => u.Id == id).SingleOrDefault();

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found product with id {id.ToString()}", "");
                }
               if(unitInStock != null) response.UnitInStock =(int) unitInStock;
                if(status !=null)response.Status=(int)status;
                _unitOfWork.Repository<Product>().Update(response, id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ProductResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update Product Error!!!", ex.InnerException?.Message);
            }
        }
    }
}
