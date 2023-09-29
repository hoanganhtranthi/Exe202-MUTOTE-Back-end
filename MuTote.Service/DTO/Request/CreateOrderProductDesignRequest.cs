using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CreateOrderProductDesignRequest
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string Img { get; set; }
    }
}
