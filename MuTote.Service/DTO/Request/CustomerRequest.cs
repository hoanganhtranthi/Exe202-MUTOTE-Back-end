using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CustomerRequest
    {
        public string? Name { get; set; } 
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? GoogleId { get; set; }
        public int? Status { get; set; }

    }
}
