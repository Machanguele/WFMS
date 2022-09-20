using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Components.Commands.RequestModel;
using Application.Features.Components.Queries.RequestModel;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ComponentsController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Component>> AddComponent(CreateComponentCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Component>>> GetComponents(LisAllComponents.ListAllComponentsQuery listAllComponentsQuery)
        {
            return await Mediator.Send(listAllComponentsQuery);
        }
        
         [HttpGet("{id}")]
        public async Task<ActionResult<List<Component>>> GetComponentsByDepartment(int Id)
        {
            
            return await Mediator.Send(new ListComponentsByDepartment.ListComponentsByDepartmentQuery(Id));
        } 
        
        [HttpGet("groupDepartments")]
        public async Task<ActionResult<List<ComponentsGroupDeptDto>>> GetByDepartment(LisAllComponentsGroupByDpt.
            LisAllComponentsGroupByDptQuery allComponentsGroupByDptQuery)
        {
            
            return await Mediator.Send(allComponentsGroupByDptQuery);
        }
        
        
    }
}