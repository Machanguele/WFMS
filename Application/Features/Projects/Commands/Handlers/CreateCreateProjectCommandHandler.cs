using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Projects.Commands.RequestModel;
using Application.Interfaces;
using Application.Specifications;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Projects.Commands.Handlers
{
    public class CreateCreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCreateProjectCommandHandler(IUserAccessor userAccessor, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userAccessor = userAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var spec = new ProjectSpecification(request.Name);
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpec(spec);

            if (project != null)
                throw new WebException($"Project with name: {project.Name} is already registered" );

            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId);

            var projectStatus = await _unitOfWork.Repository<ProjectStatus>().GetByIdAsync(request.StatusId);
            if(projectStatus == null)throw new WebException($"Project status: {request.StatusId} not found" );

            project = new Project
            {
                Status = projectStatus,
                Description = request.Description,
                Department = department,
                Name = request.Name,
                CreatedAt = DateTime.Now,
                EndAt = request.EndAt,
                StartAt = request.StartAt,
            };
            
            _unitOfWork.Repository<Project>().AddAsync(project);
            if (await _unitOfWork.Complete() > 0) return project;
            throw new WebException("There is/are error(s) occured");

        }
    }
}