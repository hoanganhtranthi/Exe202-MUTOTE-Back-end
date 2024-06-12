

using MuToe.Application.DTO.Response;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.Application.Services.ISerive
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
        Task<PagedResults<OrderResponse>> GetOrders(OrderRequest request,PagingRequest paging);
        Task<OrderResponse> GetOrderById(int orderId);
        Task<OrderResponse> GetToUpdateOrderStatus(int orderId);
        Task<OrderReportResponse> GetOrdersReport(ReportOption option, int MonthOrQuarter, int year);
    }
}
