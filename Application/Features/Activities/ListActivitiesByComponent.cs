using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class ListActivitiesByComponent
    {
        public class ListActivitiesByComponentQuery : IRequest<List<ActivitiesbyStatusDto>>
        {
            public ListActivitiesByComponentQuery(int id)
            {
                ComponentId = id;
            }
            public int ComponentId { get; set; }
        }
        public class ListActivitiesByComponentQueryHandler : IRequestHandler<ListActivitiesByComponentQuery, List<ActivitiesbyStatusDto>>
        {
            public DataContext Context { get; }

            public ListActivitiesByComponentQueryHandler(DataContext context)
            {
                Context = context;
            }
            public async Task<List<ActivitiesbyStatusDto>> Handle(ListActivitiesByComponentQuery request, CancellationToken cancellationToken)
            {
                var activities =  await Context.Activities
                    .Include(x=>x.Component)
                    .Include(x=>x.Status)
                    .Where(x=>x.Component.Id== request.ComponentId)
                    .ToListAsync();

                var statuses = await Context.ActivityStatuses.ToListAsync(cancellationToken: cancellationToken);
                var listToReturn = new List<ActivitiesbyStatusDto>();
                foreach (var status in statuses)
                {
                    var auxActivities = activities.Where(x => x.Status.Id == status.Id).ToList();
                    var statusActivities = new List<ActivitiesDto>();
                    foreach (var item in auxActivities)
                    {
                        statusActivities.Add(new ActivitiesDto()
                        {
                            Description = item.Description,
                            Name = item.Name,
                            Id = item.Id,
                            Status = item.Status,
                            AllocatedTo = item.AllocatedTo?.FullName,
                            CreatedAt = item.CreatedAt.ToString("yyyy-MM-dd"),
                            StarAt = item.StarAt.ToString("yyyy-MM-dd"),
                            EndAt = item.EndAt.ToString("yyyy-MM-dd"),
                            ExpectedStarDate = item.ExpectedStarAt.ToString("yyyy-MM-dd"),
                            ExpectedEndDate = item.ExpectedEndAt.ToString("yyyy-MM-dd"),
                        });
                    }
                    listToReturn.Add(new ActivitiesbyStatusDto()
                    {
                        Status = status,
                        TotalActivities = activities.Count,
                        Activities = statusActivities
                    });
                }

                return listToReturn;
            }
        }
    }
}