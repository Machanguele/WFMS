using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class AddActivityFile
    {
        public class AddActivityFileCommand : IRequest<Activity>
        {
            public IFormFile File { get; set; }
            public int ActivityId { get; set; }
        }

        public class AddActivityFileCommandValidator : AbstractValidator<AddActivityFileCommand>
        {
            public AddActivityFileCommandValidator()
            {
                RuleFor(x => x.File).NotEmpty();
                RuleFor(x => x.ActivityId).NotEmpty();
            }
        }
        
        public class AddActivityFileCommandHandler : IRequestHandler<AddActivityFileCommand, Activity>
        {
            private readonly DataContext _context;

            public AddActivityFileCommandHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<Activity> Handle(AddActivityFileCommand request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.ActivityId);
                if (activity == null)
                {
                    throw  new WebException("Activity not found",
                        (WebExceptionStatus) HttpStatusCode.NotFound);
                }
                
                
                throw new System.NotImplementedException();
            }
        }
    }
}