using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Employee = new HashSet<Employee>();
        }

        public int RolesId { get; set; }
        public string RolesName { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
