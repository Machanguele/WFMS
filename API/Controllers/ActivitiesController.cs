using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Activities;
using Application.Features.Users.Commands.Handlers;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Activity>> AddActivity(CreateActivity.CreateActivityCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Activity>>> GEtActivitiesByComponent(int Id)
        {
            
            return await Mediator.Send(new ListActivitiesByComponent.ListActivitiesByComponentQuery(Id));
        } 
        
        [Consumes("multipart/form-data")]
        [HttpPost("loudFile")]
        public async Task<ActionResult<string>> LoudActivitiesByComponent( [FromForm]IFormFile file, [FromForm]string componentId)
        {
            var compId = Int32.Parse(componentId);
            var command = new LoudActivitiesFromFile.LoudActivitiesFromFileCommand(file, compId);
            
            return await Mediator.Send(command);
        } 

        
    }
}