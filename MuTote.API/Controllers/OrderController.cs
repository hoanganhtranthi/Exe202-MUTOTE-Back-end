using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuToe.Application.DTO.Response;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.Services.ISerive;
using System.Net;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Get list of orders
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResults<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders([FromQuery] PagingRequest pagingRequest, [FromQuery] OrderRequest orderRequest)
        {
            var rs = await _orderService.GetOrders(orderRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrder(int id)
        {
            var rs = await _orderService.GetOrderById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet("{ordId:int}/finished-order")]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetToUpdateStatus(int ordId)
        {
            var rs = await _orderService.GetToUpdateOrderStatus(ordId);
            return Ok(rs);
        }
        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest order)
        {
            var rs = await _orderService.CreateOrder(order);
            return Ok(rs);
        }
        /// <summary>
        /// Get reports on admin orders (  Month=1,  Quarter=0)
        /// </summary>
        /// <returns></returns>
       [Authorize(Roles = "admin")]
        [HttpGet("order-report")]
        [ProducesResponseType(typeof(OrderReportResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetReportOrder(ReportOption option , int MonthsOrQuarter , int year)
        {
            var rs = await _orderService.GetOrdersReport(option,MonthsOrQuarter,year);
            return Ok(rs);
        }
    }
}
