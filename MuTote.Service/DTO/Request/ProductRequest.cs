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
        public int? UnitInStock { get; set; }
        public int? CategoryProductId { get; set; }
    }
}
