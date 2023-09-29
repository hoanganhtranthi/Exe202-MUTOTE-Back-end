using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
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
