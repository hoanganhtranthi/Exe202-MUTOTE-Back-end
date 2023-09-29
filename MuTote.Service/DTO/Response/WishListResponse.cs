using MuTote.Data.Enities;
using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class WishListResponse
    {
        [Key]
        public int Id { get; set; }
        [IntAttribute]
        public int? CustomerId { get; set; }
        [IntAttribute]
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImg { get; set; }
    }
}
