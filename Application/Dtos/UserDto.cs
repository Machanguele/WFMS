using Domain;

namespace Application.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int Id { set; get; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public bool Archived { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
    }
}