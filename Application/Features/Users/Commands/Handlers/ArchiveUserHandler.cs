using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users.Commands.RequestModels;
using Domain;
using MediatR;

namespace Application.Features.Users.Commands.Handlers
{
    public class ArchiveUserHandler : IRequestHandler<ArchiveUserCommand, UserDto>
    {
        public Task<UserDto> Handle(ArchiveUserCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}