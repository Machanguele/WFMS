using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users.Commands.RequestModels;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class UsersController : BaseController
    {
        [HttpPost]
        //[Authorize(Roles = "CRUDUser")]
        public async Task<ActionResult<UserDto>> AddUser(CreateUserCommand userCommand)
        {
            return await Mediator.Send(userCommand); 
        }
    }
}