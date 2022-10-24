using Domain;

namespace Application.Dtos
{
    public class ActivitiesDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string StarAt { get; set; }
        public string EndAt { get; set; }
        public string ExpectedStarDate { get; set; }
        public string ExpectedEndDate { get; set; }
        public string  CreatedBy { get; set; }
        public string  AllocatedTo { get; set; }
        public virtual ActivityStatus Status { get; set; }
        public virtual ActivityType Type { get; set; }
    }
    
}