using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users;
using Application.Features.Users.Commands.RequestModels;
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
        
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> ListAllUsers()
        {
            
            return await Mediator.Send(new ListUsers.ListUsersQuery());
        } 
    }
}