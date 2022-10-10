using System;

namespace Domain
{
    public class Activity : SetUpEntity
    {
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpectedStarAt { get; set; }
        public DateTime ExpectedEndAt { get; set; }
        public DateTime StarAt { get; set; } 
        public DateTime EndAt { get; set; }
        public virtual AppUser CreatedBy { get; set; }
        public virtual AppUser AllocatedTo { get; set; }
        public virtual ActivityStatus Status { get; set; }
        public virtual ActivityType Type { get; set; }
        public virtual Component Component { get; set; }
    }
}