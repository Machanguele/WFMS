using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class GanttActivitiesDto
    {
        public ComponentDto Component { get; set; }
        public List<ActivitiesDto> Activities { get; set; }
    }
}