using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class ListActivitiesByComponent
    {
        public class ListActivitiesByComponentQuery : IRequest<List<Activity>>
        {
            public ListActivitiesByComponentQuery(int id)
            {
                ComponentId = id;
            }
            public int ComponentId { get; set; }
        }
        public class ListActivitiesByComponentQueryHandler : IRequestHandler<ListActivitiesByComponentQuery, List<Activity>>
        {
            public DataContext Context { get; }

            public ListActivitiesByComponentQueryHandler(DataContext context)
            {
                Context = context;
            }
            public async Task<List<Activity>> Handle(ListActivitiesByComponentQuery request, CancellationToken cancellationToken)
            {
                return await Context.Activities
                    .Include(x=>x.Component)
                    .Where(x=>x.Component.Id== request.ComponentId)
                    .ToListAsync();
            }
        }
    }
}