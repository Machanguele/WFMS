using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class ActivitiesbyStatusDto
    {
        public ActivityStatus Status { get; set; }
        public int TotalActivities { get; set; }
        public List<ActivitiesDto> Activities { get; set; }
    }
}