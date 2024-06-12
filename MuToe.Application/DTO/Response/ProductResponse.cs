

using MuTote.Domain.Commons;

namespace MuTote.Application.DTO.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        [StringAttribute]
        public string? Name { get; set; } = null!;
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int? UnitInStock { get; set; }
        [IntAttribute]
        public int? CategoryProductId { get; set; }
        [IntAttribute]
        public int? Status { get; set; }
        [BooleanAttribute]
        public bool? IsBestSeller { get; set; }
        public virtual CategoryResponse CategoryProduct { get; set; }
    }
}
