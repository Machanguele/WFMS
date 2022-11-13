using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Commands.Handlers
{
    public class CloseComponent
    {
        public class CloseComponentCommand : IRequest<Component>
        {
            public int ComponentId { get; set; }
        }
        public class CloseComponentHandler : IRequestHandler<CloseComponentCommand, Component>
        {
            private readonly DataContext _context;

            public CloseComponentHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<Component> Handle(CloseComponentCommand request, CancellationToken cancellationToken)
            {
                var component = await _context.Components
                    .FirstOrDefaultAsync(x => x.Id == request.ComponentId,
                        cancellationToken: cancellationToken);
                if (component != null)
                {
                    component.Finished = true;
                    _context.Entry(component).State = EntityState.Modified;
                    var result = await _context.SaveChangesAsync(cancellationToken);
                }

                return component;
            }
        }
    }
}