using Application.Dtos;
using MediatR;

namespace Application.Features.Users.Commands.RequestModels
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
    }
}