using Domain;
using MediatR;

namespace Application.Features.UsersSystem.commands.RequestModels
{
    public class CreateUsersSystemsCommand : IRequest<UserSystem>
    {
        public string UserEmail { get; set; }
        public int EcoSystemId{ get; set; }
    }
}