using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class WishListRequest
    {
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
    }
    public class CreateWishListRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int CustomerId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Quantity { get; set; }
    }
}
