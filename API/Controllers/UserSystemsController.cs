using System.Threading.Tasks;
using Application.Features.UsersSystem.commands.RequestModels;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserSystemsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<UserSystem>> AddUserSystem(CreateUsersSystemsCommand command)
        {
            return await Mediator.Send(command); 
        }
        
    }
}