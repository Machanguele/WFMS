using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class CreateActivity
    {
        public class CreateActivityCommand : IRequest<Activity>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int ActivityTypeId { get; set; }
            public int ComponentId { get; set; }
            public DateTime ExpectedStarAt { get; set; }
            public DateTime ExpectedEndAt { get; set; }
        }

        public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
        {
            public CreateActivityCommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.ComponentId).NotEmpty();
                RuleFor(x => x.ExpectedEndAt).NotEmpty();
                RuleFor(x => x.ExpectedStarAt).NotEmpty();
            }
        }

        public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Activity>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;

            public CreateActivityCommandHandler(DataContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }
            public async Task<Activity> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
            {
                var status = await _context.ActivityStatuses.FirstOrDefaultAsync(x => x.Name == "Por fazer", cancellationToken: cancellationToken);
                if(status == null)
                    throw new WebException("Status not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                var component = await _context.Components.FirstOrDefaultAsync(x => x.Id == request.ComponentId, cancellationToken: cancellationToken);
                if(component == null)
                    throw new WebException("Component not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                
                var activityType = await _context.ActivityTypes.FirstOrDefaultAsync(x => x.Id == request.ActivityTypeId, cancellationToken: cancellationToken);

                /*if (request.ExpectedEndAt < request.ExpectedStarAt)
                {
                    throw new WebException("A data esperada de termino deve ser maior ou igual que a data esperada de inicio", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }
                if (request.ExpectedStarAt < component.ExpectedStartDate || request.ExpectedStarAt > component.ExpectedEndDate)
                {
                    throw new WebException("A data esperada nao deve estar fora do escopo do componente", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }*/
                /*if (request.ExpectedEndAt  < component.ExpectedStartDate || request.ExpectedEndAt >component.ExpectedEndDate)
                {
                    throw new WebException("A data esperada nao deve estar fora do escopo do componente", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }*/

                var activity = new Activity()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Status = status,
                    Component = component,
                    CreatedBy = await _userManager.Users.FirstOrDefaultAsync(x =>
                        x.Email == _userAccessor.GetCurrentUserEmail(), cancellationToken: cancellationToken),
                    Type = activityType,
                    ExpectedStarAt = request.ExpectedStarAt,
                    ExpectedEndAt = request.ExpectedEndAt
                };

                await _context.AddAsync(activity);
                if (await _context.SaveChangesAsync(cancellationToken) > 0) return activity;
                throw  new WebException("Fail tto save activity",
                    (WebExceptionStatus) HttpStatusCode.BadRequest);
            }
        }
    }
}