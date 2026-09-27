using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LoginResponse
    {
        public bool sucesso { get; set; }
        public string token { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string mensagem { get; set; }
    }
}
