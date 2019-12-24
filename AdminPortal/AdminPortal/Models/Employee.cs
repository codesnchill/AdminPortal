using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Account = new HashSet<Account>();
        }

        public string EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeBio { get; set; }
        public string Roles { get; set; }

        public int Points { get; set; }
        public int RolesId { get; set; }
        public string ImageUrl { get; set; }

        public string IsDisabled { get; set; }

        public string DepartmentName { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}
