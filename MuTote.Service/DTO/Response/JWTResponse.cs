using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class JWTResponse
    {
        public string? Token { get; set; }
        public CustomerResponse Customer { get; set; }
    }
}
