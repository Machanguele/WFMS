using System;
using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class ComponentDto
    {
        public int Id { get; set; }
        public int FinishedActivities { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string ExpectedStartDate { get; set; }
        public string ExpectedEndDate { get; set; }
        public string StartedDate { get; set; }
        public string ActualEndDate { get; set; }
        public virtual AppUser CreatedBy { get; set; }
        public virtual ComponentStatus ComponentStatus { get; set; }
        public virtual Department Department { get; set; }
        public bool Finished { get; set; }
        public List<Activity> Activities { get; set; }
    }
}