using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class UpdateActivityStatus
    {
        public class UpdateActivityStatusCommand : IRequest<Activity>
        {
            public int ActivityId { get; set; }
            public string ActivityStatus { get; set; }
        }

        public class UpdateActivityStatusCommandValidator : AbstractValidator<UpdateActivityStatusCommand>
        {
            public UpdateActivityStatusCommandValidator()
            {
                RuleFor(x => x.ActivityId).NotEmpty();
                RuleFor(x => x.ActivityStatus).NotEmpty();
            }
        }

        public class UpdateActivityStatusHandler : IRequestHandler<UpdateActivityStatusCommand, Activity>
        {
            private readonly DataContext _context;

            public UpdateActivityStatusHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<Activity> Handle(UpdateActivityStatusCommand request, CancellationToken cancellationToken)
            {
                var status = await _context.ActivityStatuses
                    .FirstOrDefaultAsync(x => x.Name.ToLower() == request.ActivityStatus.ToLower(), cancellationToken: cancellationToken);

                var activity = await _context.Activities
                    .Include(x=>x.Component)
                    .FirstAsync(x => x.Id == request.ActivityId, cancellationToken: cancellationToken);

                if (activity == null || status == null)
                {
                    throw new WebException("Status not found", 
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }

                activity.Status = status;
                _context.Entry(activity).State = EntityState.Modified;

                var activities = await _context.Activities
                    .Include(x => x.Status)
                    .Include(x => x.Component)
                    .Where(x => x.Status.Name != "Concluídas" && x.Component.Id == activity.Component.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if(activities != null)
                    activities.Component.Finished = true;
                if (activities != null) _context.Entry((object) activities.Component).State = EntityState.Modified;
                
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                    return activity;
                throw new WebException("Status not found", 
                    (WebExceptionStatus) HttpStatusCode.NotFound);

            }
        }
    }
}