

using MuTote.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace MuTote.Application.DTO.Response
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
