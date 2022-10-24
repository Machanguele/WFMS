using System;
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
    public class ListActivitiesByGantt
    {
        public class ListActivitiesByGanttQuery : IRequest<GanttActivitiesDto>
        {
            public ListActivitiesByGanttQuery(int id)
            {
                ComponentId = id;
            }
            public int ComponentId { get; set; }
        }
        public class ListActivitiesByGanttQueryHandler : IRequestHandler<ListActivitiesByGanttQuery, GanttActivitiesDto>
        {
            public DataContext Context { get; }

            public ListActivitiesByGanttQueryHandler(DataContext context)
            {
                Context = context;
            }
            public async Task<GanttActivitiesDto> Handle(ListActivitiesByGanttQuery request, CancellationToken cancellationToken)
            {
                var component = await Context.Components.FirstOrDefaultAsync(x => x.Id == request.ComponentId, cancellationToken: cancellationToken);
                var activities =  await Context.Activities
                    .Include(x=>x.Component)
                    .Include(x=>x.Status)
                    .Include(x=>x.AllocatedTo)
                    .Where(x=>x.Component.Id== request.ComponentId)
                    .ToListAsync(cancellationToken: cancellationToken);

                var listToReturn = new List<ActivitiesDto>();
                
                    foreach (var item in activities)
                    {
                        listToReturn.Add(new ActivitiesDto
                        {
                            Description = item.Description,
                            Name = item.Name,
                            Id = item.Id,
                            Status = item.Status,
                            AllocatedTo = item.AllocatedTo?.FullName,
                            CreatedAt = item.CreatedAt.ToString("yyyy-MM-dd"),
                            StarAt = item.StarAt.Year != 0001? item.StarAt.ToString("yyyy-MM-dd") : "-",
                            EndAt = item.EndAt.Year != 00001? item.EndAt.ToString("yyyy-MM-dd") :"-",
                            ExpectedStarDate = item.ExpectedStarAt.ToString("yyyy-MM-dd"),
                            ExpectedEndDate = item.ExpectedEndAt.ToString("yyyy-MM-dd"),
                        });
                    }
                    
                

                return new GanttActivitiesDto
                {
                      Activities = listToReturn,
                      Component = new ComponentDto
                        {
                            Id = component.Id,
                            Department = component.Department,
                            Description = component.Description,
                            Title = component.Title,
                            ComponentStatus = component.ComponentStatus,
                            CreatedBy = component.CreatedBy,
                            CreatedAt = DateHelper(component.CreatedAt),
                            StartedDate = DateHelper(component.StartedDate),
                            ActualEndDate = DateHelper(component.ActualEndDate),
                            ExpectedEndDate = DateHelper(component.ExpectedEndDate),
                            ExpectedStartDate = DateHelper(component.ExpectedStartDate),
                            FinishedActivities = activities.Count >0?  activities.Count(x => x.Status.Description == "Concluido"): 0
                        }
                };
            }
            public string DateHelper(DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
        }
    }
}