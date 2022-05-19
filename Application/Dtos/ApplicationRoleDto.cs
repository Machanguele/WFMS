using System.Collections.Generic;
using Domain;

namespace Application.Dtos
{
    public class ApplicationRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ApplicationPermission> ApplicationPermissions { get; set; }
    }
}