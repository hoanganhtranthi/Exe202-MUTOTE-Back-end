using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MuTote.Data.Enities;
using MuTote.Data.UnitOfWork;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Exceptions;
using MuTote.Service.Services.ISerive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MuTote.Service.Helpers.Enum;
using Twilio.TwiML.Voice;
using AutoMapper.QueryableExtensions;
using MuTote.Service.Helpers;
using MuTote.Service.Utilities;
using static System.Formats.Asn1.AsnWriter;

namespace MuTote.Service.Services.ImpService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {    
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    order.Status = (int)OrderStatusEnum.Pending; 
                    order.TotalPrice= 0;
                    order.CustomerId = request.CustomerId;
                    order.Customer= _unitOfWork.Repository<Customer>().Find(x => x.Id == request.CustomerId);
                               
                    #region Order detail
                    List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                    List<OrderDetailResponse> listOrderDetailResponse = new List<OrderDetailResponse>();
                    foreach (var detail in request.OrderDetails)
                    {
                    OrderDetail orderDetail = new OrderDetail();
                     var product = _unitOfWork.Repository<Product>().GetAll()
                                .Include(x => x.CategoryProduct)
                                .Where(x => x.Id == detail.ProductId)
                                .FirstOrDefault();
                    if (product.UnitInStock < detail.Quantity) throw new CrudException(HttpStatusCode.BadRequest, "Not enough quantity", "");

                    var wishList=_unitOfWork.Repository<WishList>().GetAll().Where(a=>a.CustomerId==request.CustomerId && a.ProductId==detail.ProductId).SingleOrDefault();
                    if (wishList != null) _unitOfWork.Repository<WishList>().Delete(wishList);

                    order.TotalPrice += (decimal)(detail.Quantity * product.Price);   
                    orderDetail.Product = product;
                    orderDetail.Quantity = detail.Quantity;
                    orderDetail.ProductId = detail.ProductId;
                    listOrderDetail.Add(orderDetail);
                     var orderDetailResult = _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
                     orderDetailResult.Img = product.Img;
                    orderDetailResult.Name = product.Name;
                    orderDetailResult.Price= (decimal)(detail.Quantity * product.Price);
                    product.UnitInStock -= (int)detail.Quantity;
                    listOrderDetailResponse.Add(orderDetailResult);
                    }
                    #endregion
                    order.OrderDetails = listOrderDetail;

                    await _unitOfWork.Repository<Order>().CreateAsync(order);
                    await _unitOfWork.CommitAsync();

                    var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);

                    var orderResult = _unitOfWork.Repository<Order>().GetAll()
                        .Include(x => x.OrderDetails)
                        .Select(x => new OrderResponse()
                        {
                            Id = x.Id,
                            CustomerId = x.CustomerId,
                            Customer =customerResult,
                            OrderDate = x.OrderDate,
                            Status = x.Status,
                            TotalPrice= order.TotalPrice,
                            OrderDetails=listOrderDetailResponse,
                        })
                        .FirstOrDefault();
                return orderResult;

            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Order Error!!!", e.InnerException?.Message);
            }
        }

        public async Task<OrderResponse> CreateOrderProductDesign(CreateOrderProductDesignRequest request)
        {
            try
            {
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.Status = (int)OrderStatusEnum.Pending;
                order.TotalPrice = 150000;
                order.CustomerId = request.CustomerId;
                order.Customer = _unitOfWork.Repository<Customer>().Find(x => x.Id == request.CustomerId);

                #region Order detail
                List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                List<OrderDetailResponse> listOrderDetailResponse = new List<OrderDetailResponse>();
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Quantity = 1;
                listOrderDetail.Add(orderDetail);
                var orderDetailResult = _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
                orderDetailResult.Img = request.Img;
                orderDetailResult.Name = "Designed Product";
                orderDetailResult.Price = 150000;
                listOrderDetailResponse.Add(orderDetailResult);
                #endregion
                order.OrderDetails = listOrderDetail;

                await _unitOfWork.Repository<Order>().CreateAsync(order);
                await _unitOfWork.CommitAsync();

                var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);

                var orderResult = _unitOfWork.Repository<Order>().GetAll()
                    .Include(x => x.OrderDetails)
                    .Select(x => new OrderResponse()
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Customer = customerResult,
                        OrderDate = x.OrderDate,
                        Status = x.Status,
                        TotalPrice = order.TotalPrice,
                        OrderDetails = listOrderDetailResponse,
                    })
                    .FirstOrDefault();
                return orderResult;

            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Order Error!!!", e.InnerException?.Message);
            }
        }

        public async Task<OrderResponse> GetOrderById(int orderId)
        {
            try
            {
                if (orderId <= 0)
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Order Invalid", "");
                var order = _unitOfWork.Repository<Order>().GetAll().Where(a => a.Id == orderId).Select(a => new OrderResponse
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    OrderDate = a.OrderDate,
                    Customer = _mapper.Map<CustomerResponse>(a.Customer),
                    TotalPrice = a.TotalPrice,
                    Status = a.Status,
                    OrderDetails = new List<OrderDetailResponse>
                   (a.OrderDetails.Select(x => new OrderDetailResponse
                   {
                       Id = x.Id,
                       ProductId = x.ProductId,
                       Quantity = x.Quantity,
                       Price = (_unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Price) * x.Quantity,
                       Img = _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Img,
                       Name = _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Name,
                       OrderId = x.OrderId
                   }))
                }).SingleOrDefault();
                if (order == null)
                    throw new CrudException(HttpStatusCode.NotFound, "Order Id not found", orderId.ToString());
                return order;
            }
            catch (CrudException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", ex.Message);
            }
        }

        public async Task<PagedResults<OrderResponse>> GetOrders(OrderRequest request, PagingRequest paging)
        {
            try
            {

                var filter = _mapper.Map<OrderResponse>(request);
                var orders =  _unitOfWork.Repository<Order>().GetAll().Select(a => new OrderResponse
                {
                    Id = a.Id,                   
                    CustomerId = a.CustomerId,
                    OrderDate = a.OrderDate,
                    Customer = _mapper.Map<CustomerResponse>(a.Customer),
                    TotalPrice = a.TotalPrice,    
                    Status = a.Status,
                    OrderDetails = new List<OrderDetailResponse>
                   (a.OrderDetails.Select(x => new OrderDetailResponse
                   {
                       Id=x.Id,
                       ProductId=x.ProductId,
                       Quantity=x.Quantity,
                       Price=(_unitOfWork.Repository<Product>().GetAll().Where(a=>a.Id==x.ProductId).SingleOrDefault().Price)*x.Quantity,
                       Img= _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Img,
                       Name= _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Name,
                       OrderId=x.OrderId
                   }))
                }).DynamicFilter(filter).ToList();
                var sort = PageHelper<OrderResponse>.Sorting(paging.SortType, orders, paging.ColName);
                var result = PageHelper<OrderResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get order list error!!!!!", ex.Message);
            }
        }

        public async Task<OrderResponse> GetToUpdateOrderStatus(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetAll().Include(c=>c.OrderDetails).Include(c=>c.Customer)
                            .Where(x => x.Id == orderId)
                            .FirstOrDefaultAsync();
                if (orderId <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Order Invalid", "");
                }
                if (order == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found order with id {orderId.ToString()}", "");
                }
                order.Status = (int)OrderStatusEnum.Finish;

                await _unitOfWork.Repository<Order>().Update(order,orderId);
                await _unitOfWork.CommitAsync();

                var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);

                var orderResult = _unitOfWork.Repository<Order>().GetAll()
                    .Include(x => x.OrderDetails)
                    .Select(x => new OrderResponse()
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Customer = customerResult,
                        OrderDate = x.OrderDate,
                        Status = x.Status,
                        TotalPrice = order.TotalPrice,
                        OrderDetails = new List<OrderDetailResponse>
                   (x.OrderDetails.Select(x => new OrderDetailResponse
                   {
                       Id = x.Id,
                       ProductId = x.ProductId,
                       Quantity = x.Quantity,
                       Price = (_unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Price) * x.Quantity,
                       Img = _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Img,
                       Name = _unitOfWork.Repository<Product>().GetAll().Where(a => a.Id == x.ProductId).SingleOrDefault().Name,
                       OrderId = x.OrderId
                   }))  })    .FirstOrDefault();
                return orderResult;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", e.Message);
            }
        }
    }
}
