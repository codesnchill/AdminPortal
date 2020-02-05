using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class Token
    {
        public string Token1 { get; set; }
        public string Refresh_token { get; set; }
        public string Token_expiration { get; set; }
    }
}
