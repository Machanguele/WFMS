using System;
using Domain;
using MediatR;

namespace Application.Features.Components.Commands.RequestModel
{
    public class CreateComponentCommand : IRequest<Component>
    {
        
        public string Title { get; set; } 
        public string Description { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int DepartmentId { get; set; }
    }
}