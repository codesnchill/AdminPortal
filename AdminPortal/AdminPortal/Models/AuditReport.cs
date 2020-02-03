using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class AuditReport
    {
        public string AdminName { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Target { get; set; }
    }
}
