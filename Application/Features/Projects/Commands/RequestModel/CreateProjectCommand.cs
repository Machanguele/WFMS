using System;
using Domain;
using MediatR;

namespace Application.Features.Projects.Commands.RequestModel
{
    public class CreateProjectCommand : IRequest<Project>
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int StatusId { get; set; }
        public int DepartmentId { get; set; }
    }
}