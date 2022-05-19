using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.EcoSystems.Commands.RequestModels;
using Application.Features.EcoSystems.Queries.RequestModels;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EcosystemsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<EcoSystem>> AddSystem(CreateEcosystemCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet]
        public async Task<IReadOnlyList<EcoSystem>> GetAllSystems()
        {
            return await Mediator.Send(new GelAllEcoSystemsQuery());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<EcoSystem>> GetAllSystemById(int id)
        {
            return await Mediator.Send(new GetEcoSystemByIdQuery(id));
        } 
        [HttpPut("{id}")]
        public async Task<ActionResult<EcoSystem>> EditEcoSystem(UpdateEcosystemCommand command, int id)
        {
            command.EcosystemId = id;
            return await Mediator.Send(command);
        }

       
        
    }
}