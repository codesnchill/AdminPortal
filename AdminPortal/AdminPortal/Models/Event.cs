using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class Event
    {
        public Event()
        {
            DepartmentEvent = new HashSet<DepartmentEvent>();
        }

        public int EventId { get; set; }

        public string DepartmentName { get; set; }

        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public byte[] UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

        public ICollection<DepartmentEvent> DepartmentEvent { get; set; }
    }
}
