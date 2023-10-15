using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Services.ISerive;
using static MuTote.Service.Helpers.Enum;

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
        //[Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders([FromQuery] PagingRequest pagingRequest, [FromQuery] OrderRequest orderRequest)
        {
            var rs = await _orderService.GetOrders(orderRequest, pagingRequest);
            return Ok(rs);
        }
        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       //[Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int id)
        {
            var rs = await _orderService.GetOrderById(id);
            return Ok(rs);
        }
        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       //[Authorize(Roles = "admin")]
        [HttpGet("{ordId:int}/finished-order")]
        public async Task<ActionResult<OrderResponse>> GetToUpdateStatus(int ordId)
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
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest order)
        {
            var rs = await _orderService.CreateOrder(order);
            return Ok(rs);
        }
        /// <summary>
        /// Get reports on admin orders (  Month=1,  Quarter=0)
        /// </summary>
        /// <returns></returns>
       //[Authorize(Roles = "admin")]
        [HttpGet("order-report")]
        public async Task<ActionResult<dynamic>> GetReportOrder(ReportOption option , int MonthsOrQuarter , int year)
        {
            var rs = await _orderService.GetOrdersReport(option,MonthsOrQuarter,year);
            return Ok(rs);
        }
    }
}
