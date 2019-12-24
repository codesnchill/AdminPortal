using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class DepartmentEvent
    {
        public int DepartmentId { get; set; }
        public int EventId { get; set; }

        public Department Department { get; set; }
        public Event Event { get; set; }
    }
}
