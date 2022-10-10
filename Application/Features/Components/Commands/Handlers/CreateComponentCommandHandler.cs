using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Components.Commands.RequestModel;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Commands.Handlers
{
    public class CreateComponentCommandHandler : IRequestHandler<CreateComponentCommand, Component>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public CreateComponentCommandHandler(IUserAccessor userAccessor, UserManager<AppUser> userManager, DataContext
            context)
        {
            _userAccessor = userAccessor;
            _userManager = userManager;
            _context = context;
        }
        public async Task<Component> Handle(CreateComponentCommand request, CancellationToken cancellationToken)
        {
            var status = await _context.ComponentStatus.FirstOrDefaultAsync(x => x.Name == "Por iniciar");
            if(status == null) throw new WebException("Status was not found", (WebExceptionStatus) 
                HttpStatusCode.BadRequest);
            
            var dept = await _context.Departments.FirstOrDefaultAsync(x => x.Id == request.DepartmentId, cancellationToken: cancellationToken);
            if (dept == null) throw new WebException("Department was not found", (WebExceptionStatus)
                HttpStatusCode.NotFound);

            var component = await _context.Components
                .FirstOrDefaultAsync(x => x.Title.Trim().ToUpper()
                .Equals(request.Title.Trim().ToUpper()) && x.Department.Id == request.DepartmentId, cancellationToken: cancellationToken);
            if(component != null) throw new WebException("Component is already exists", 
                (WebExceptionStatus) HttpStatusCode.BadRequest);

            var currentUser = await _userManager.Users.Where(x => x.Email == _userAccessor
                .GetCurrentUserEmail()).FirstOrDefaultAsync();
            
            component = new Component()
            {
                Description = request.Description,
                Title = request.Title,
                Department = dept,
                ExpectedStartDate = request.ExpectedStartDate,
                ExpectedEndDate = request.ExpectedEndDate,
                CreatedBy = currentUser,
                ComponentStatus = status,
                CreatedAt = DateTime.Now,
            };

            await _context.AddAsync(component);
            
            if (await _context.SaveChangesAsync(cancellationToken) > 0) return component;
            throw  new WebException("Fail to Login",
                (WebExceptionStatus) HttpStatusCode.BadRequest);

        }
    }
}