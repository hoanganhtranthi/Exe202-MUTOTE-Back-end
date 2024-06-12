
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Request
{
    public class CreateMaterialRequest
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Img { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int CategoryMaterialId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? DesignerId { get; set; }
    }
}
