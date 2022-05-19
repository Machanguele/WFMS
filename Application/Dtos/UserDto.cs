using Domain;

namespace Application.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public Department Department { get; set; }
        public ApplicationRoleDto ApplicationRoleDto { get; set; }
    }
}