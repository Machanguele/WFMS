using System;
using System.Collections.Generic;

namespace Domain
{
    public class Component
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public virtual AppUser CreatedBy { get; set; }
        public virtual ComponentStatus ComponentStatus { get; set; }
        public virtual Department Department { get; set; }
        
    }
}