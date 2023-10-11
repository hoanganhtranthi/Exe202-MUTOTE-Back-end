using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
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
