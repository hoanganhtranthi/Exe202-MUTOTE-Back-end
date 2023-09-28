using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? CateMaterialImg { get; set; }
        public string? CateProductImg { get; set; }
    }
}