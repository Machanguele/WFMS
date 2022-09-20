using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Queries.RequestModel
{
    public class LisAllComponents
    {
        public class ListAllComponentsQuery : IRequest<List<Component>>
        {
            
        }
        public class ListAllComponentsQueryHandler : IRequestHandler<ListAllComponentsQuery, List<Component>>
        {
            private readonly DataContext _context;

            public ListAllComponentsQueryHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Component>> Handle(ListAllComponentsQuery request, CancellationToken cancellationToken)
            {
                return await _context.Components
                    .Include(x => x.Department)
                    .ToListAsync();
            }
        }
    }
}