using MuTote.Data.Enities;
using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
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
