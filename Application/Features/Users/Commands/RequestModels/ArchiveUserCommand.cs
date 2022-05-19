using Application.Dtos;
using Domain;
using MediatR;

namespace Application.Features.Users.Commands.RequestModels
{
    public class ArchiveUserCommand : IRequest<UserDto>
    {
        public ArchiveUserCommand(string email)
        {
            Email = email;
        }
        public string Email { get; set; }
    }
}