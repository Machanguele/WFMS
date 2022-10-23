using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class AllocateUser
    {
        public class AllocateUserCommand : IRequest<Activity>
        {
            public int ActivityId { get; set; }
            public string UserEmail { get; set; }
        }

        public class AllocateUserCommandValidator : AbstractValidator<AllocateUserCommand>
        {
            public AllocateUserCommandValidator()
            {
                RuleFor(x => x.ActivityId).NotEmpty();
                RuleFor(x => x.UserEmail).NotEmpty();
            }
        }

        public class AllocateUserHandler : IRequestHandler<AllocateUserCommand, Activity>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;

            public AllocateUserHandler(DataContext context, UserManager<AppUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }
            public async Task<Activity> Handle(AllocateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.UserEmail, cancellationToken: cancellationToken);
                var activity = await _context.Activities.FirstAsync(x => x.Id == request.ActivityId);

                if (user == null || activity == null)
                {
                    throw new WebException("User or activity not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }

                activity.AllocatedTo = user;
                _context.Entry(activity).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                    return activity;
                throw new WebException("Status not found", 
                    (WebExceptionStatus) HttpStatusCode.NotFound);

            }
        }
    }
}