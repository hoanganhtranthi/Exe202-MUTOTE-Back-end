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
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
        Task<OrderResponse> CreateOrderProductDesign(CreateOrderProductDesignRequest request);
        Task<PagedResults<OrderResponse>> GetOrders(OrderRequest request,PagingRequest paging);
        Task<OrderResponse> GetOrderById(int orderId);
        Task<OrderResponse> GetToUpdateOrderStatus(int orderId);
    }
}
