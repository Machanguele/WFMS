using System;

namespace Domain
{
    public class Project : SetUpEntity
    {
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public virtual Project Status { get; set; }
        public virtual Department Department { get; set; }
        public virtual AppUser CreatedBUser { get; set; }
    }
}