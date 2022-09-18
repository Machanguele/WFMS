using System.Threading.Tasks;
using Application.Features.Projects.Commands.RequestModel;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Project>> AddSystem(CreateProjectCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}