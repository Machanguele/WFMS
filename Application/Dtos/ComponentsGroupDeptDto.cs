using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class ComponentsGroupDeptDto
    {
        public Department Department { get; set; }
        public IEnumerable<Component> Components { get; set; }
        
    }
}