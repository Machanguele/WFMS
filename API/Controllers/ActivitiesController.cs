using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Activities;
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
        public async Task<ActionResult<List<ActivitiesbyStatusDto>>> GetActivitiesByComponent(int Id)
        {
            
            return await Mediator.Send(new ListActivitiesByComponent.ListActivitiesByComponentQuery(Id));
        } 
        
        [HttpPut("status")]
        public async Task<ActionResult<Activity>>UpdateStatusActivity(UpdateActivityStatus.UpdateActivityStatusCommand command)
        {
            return await Mediator.Send(command);
        } 
        
        [HttpPut("allocate")]
        public async Task<ActionResult<Activity>>AllocateUserActivity(AllocateUser.AllocateUserCommand command)
        {
            return await Mediator.Send(command);
        } 
        
        [Consumes("multipart/form-data")]
        [HttpPost("addFile")]
        public async Task<ActionResult<string>> AddACtivityFile( [FromForm]IFormFile file, [FromForm]string activityId)
        {
            var compId = Int32.Parse(activityId);
            

            return null;
            //return await Mediator.Send(command);
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