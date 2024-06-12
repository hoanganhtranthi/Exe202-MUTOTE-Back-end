
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Request
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
