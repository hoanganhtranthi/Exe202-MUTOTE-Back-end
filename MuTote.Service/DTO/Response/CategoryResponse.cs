using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class CategoryResponse
    {
        [Key]
        public int Id { get; set; }
        [StringAttribute]
        public string Name { get; set; } = null!;
        public string? CateMaterialImg { get; set; }
        public string? CateProductImg { get; set; }
    }
}
