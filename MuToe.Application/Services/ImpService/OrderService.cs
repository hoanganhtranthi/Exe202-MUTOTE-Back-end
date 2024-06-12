using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.GlobalExceptionHandling.Exceptions;
using MuTote.Application.Helpers;
using MuTote.Application.Services.ISerive;
using MuTote.Application.UnitOfWork;
using MuTote.Application.Utilities;
using MuTote.Domain.Enities;
using System.Net;

using Hangfire;
using static MuTote.Domain.Enums.Enum;
using MuToe.Application.DTO.Response;

namespace MuTote.Application.Services.ImpService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductService productService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.productService = productService;
        }
        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var cus = _unitOfWork.Repository<Customer>().Find(x => x.Id == request.CustomerId);
                 if(cus== null) throw new CrudException(HttpStatusCode.NotFound, "Customer Id not found", request.CustomerId.ToString());
                    Order order = new Order();
                    order.OrderDate = DateTime.UtcNow;
                    order.Status = (int)OrderStatusEnum.Pending; 
                    order.TotalPrice= 0;
                    order.CustomerId = request.CustomerId;
                    order.Customer= cus ;
                               
                    #region Order detail
                    List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                    List<OrderDetailResponse> listOrderDetailResponse = new List<OrderDetailResponse>();
                    foreach (var detail in request.OrderDetails)
                    {
                    if(detail.Quantity <=0) throw new CrudException(HttpStatusCode.BadRequest, "Invalid quantity", "");
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
                    listOrderDetailResponse.Add(orderDetailResult);
                    }
                    #endregion
                    order.OrderDetails = listOrderDetail;
                order.Address = request.Address;
                order.Phone = request.Phone;
                    await _unitOfWork.Repository<Order>().CreateAsync(order);
                    await _unitOfWork.CommitAsync();
                var jobId = BackgroundJob.Schedule(() => 
                    productService.GetBestSellerProduct(),
                            TimeSpan.FromMinutes(5));
                    var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);

                    var orderResult = _unitOfWork.Repository<Order>().GetAll()
                        .Include(x => x.OrderDetails)
                        .Select(x => new OrderResponse()
                        {
                            Id = x.Id,
                            Name=customerResult.Name,
                            CustomerId = request.CustomerId,
                            Customer =customerResult,
                            OrderDate = DateTime.UtcNow,
                            Phone = request.Phone,
                            Address=request.Address,
                            Status = (int)OrderStatusEnum.Pending,
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
                    Name=a.Customer.Name,
                    Address=a.Address,
                    Phone=a.Phone,
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
                var orders = _unitOfWork.Repository<Order>().GetAll().Select(a => new OrderResponse
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    OrderDate = a.OrderDate,
                    Customer = _mapper.Map<CustomerResponse>(a.Customer),
                    Name = a.Customer.Name,
                    TotalPrice = a.TotalPrice,
                    Address = a.Address,
                    Phone = a.Phone,
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

        public async Task<ProductResponse> UpdateProductQuantity(int id, int quantity)
        {
            var product =  _unitOfWork.Repository<Product>().GetAll()
                                .Include(x => x.CategoryProduct)
                                .Where(x => x.Id == id)
                                .FirstOrDefault();
            product.UnitInStock -= quantity;
            await _unitOfWork.Repository<Product>().Update(product, id);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProductResponse>(product);
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
                foreach(var ord in order.OrderDetails)
                {
                    var jobId = BackgroundJob.Schedule(() =>
                     UpdateProductQuantity((int)ord.ProductId,ord.Quantity),
                     TimeSpan.FromMinutes(1));
                }

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
                        Address = x.Address,
                        Name=x.Customer.Name,
                        Phone = x.Phone,
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
        public async Task<OrderReportResponse> GetOrdersReport(ReportOption option, int MonthOrQuarter, int year)
        {
            List<OrderDetail> orders = new List<OrderDetail>();

            if ((int)option == 1)
                orders = _unitOfWork.Repository<OrderDetail>().GetAll().Include(c => c.Product).Include(c => c.Product.CategoryProduct).Include(c => c.Order)
                        .Where(a => a.Order.OrderDate.Value.Month == MonthOrQuarter && a.Order.OrderDate.Value.Year == year).ToList();

            else {
                     var list = _unitOfWork.Repository<OrderDetail>().GetAll().Include(c => c.Product).Include(c => c.Product.CategoryProduct).Include(c => c.Order).ToList();
                      orders=list.Where(a => DateTimeUtils.GetQuarter(a.Order.OrderDate.Value) == MonthOrQuarter && a.Order.OrderDate.Value.Year == year).ToList();

            }
            var cates = new List<OrderAmoutByCateRequest>();
            foreach (var order in orders)
            {
                var cate = new OrderAmoutByCateRequest();
                var c = cates.Where(a => a.CateName.Equals(order.Product.CategoryProduct.Name)).SingleOrDefault();
                var amout = (decimal)order.Product.Price * order.Quantity;
                if (c != null)
                {
                    c.Amout += amout;
                    continue;
                }
                cate.CateName = order.Product.CategoryProduct.Name;
                cate.Total = orders.Where(a => a.Product.CategoryProduct.Name.Equals(cate.CateName)).Select(a=>a.OrderId).ToList().Distinct().Count();
                cate.Amout += amout;
                cates.Add(cate);
            }
            var ordersCate = cates.OrderByDescending(c => c.Total).ToList();
            var ordersTotal = ordersCate.Sum(x=>x.Total);
            var ordersAmout = ordersCate.Sum(x => x.Amout);
            var ordersPending = orders.Where(a => a.Order.Status == 0).Select(a => a.OrderId).Distinct().Count();
            var ordersFinish = orders.Where(a => a.Order.Status == 1).Select(a=>a.OrderId).Distinct().Count();

            return new OrderReportResponse
            {
                MonthOrQuarter=MonthOrQuarter,
                TotalOrder = ordersTotal,
                OrdersAmout=ordersAmout,    
                OrdersPending=ordersPending,
                OrdersFinish=ordersFinish,
                OrdersByCate = ordersCate
            };
        }
    }
}
