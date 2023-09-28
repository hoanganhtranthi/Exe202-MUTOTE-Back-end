using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class OrderFilterRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
