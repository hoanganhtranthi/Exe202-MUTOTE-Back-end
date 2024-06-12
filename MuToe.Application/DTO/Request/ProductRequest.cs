

namespace MuTote.Application.DTO.Request
{
    public class ProductRequest
    {
        public string? Name { get; set; }               
        public int? CategoryProductId { get; set; }
        public int? Status { get; set; }
        public bool IsBestSeller { get; set; }=false;
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

    }
}
