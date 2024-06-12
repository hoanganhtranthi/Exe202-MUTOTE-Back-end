

using MuTote.Application.DTO.Response;

namespace MuToe.Application.DTO.Response
{
    public class OrderReportResponse
    {
        public int MonthOrQuarter { get; set; }
        public int TotalOrder { get; set; }
        public decimal OrdersAmout { get; set; }
        public int OrdersPending { get; set; }
        public int OrdersFinish { get; set; }
        public List<OrderAmoutByCateRequest> OrdersByCate { get; set; }
    }
}
