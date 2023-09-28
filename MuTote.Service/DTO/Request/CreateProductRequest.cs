using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int UnitInStock { get; set; }
        public int CategoryProductId { get; set; }
    }
}
