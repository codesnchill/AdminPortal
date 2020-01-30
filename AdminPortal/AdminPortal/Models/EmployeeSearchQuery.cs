using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class EmployeeSearchQuery
    {
        public string EmployeeId { get; set; }
        public string DepartmentName { get; set; }
        public string Roles { get; set; }
        public string IsDisabled { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeName { get; set; }
    }
}
