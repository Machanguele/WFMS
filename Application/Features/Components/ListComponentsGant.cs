using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components
{
    public class ListComponentsGant
    {
        public class ListComponentsGantQuery : IRequest<List<GanttActivitiesDto>>
        {
            
        }

        public class ListComponentsGantQueryHandler : IRequestHandler<ListComponentsGantQuery, List<GanttActivitiesDto>>
        {
            private readonly DataContext _context;

            public ListComponentsGantQueryHandler(DataContext context)
            {
                _context = context;
            }
            public  async Task<List<GanttActivitiesDto>> Handle(ListComponentsGantQuery request, CancellationToken cancellationToken)
            {
                var components = await _context.Components
                    .OrderBy(x=>x.CreatedAt)
                    .ToListAsync(cancellationToken);
                
                var listToReturn = new List<GanttActivitiesDto>();
                foreach (var component in components)
                {
                    var activities =  await _context.Activities
                        .Include(x=>x.Component)
                        .Include(x=>x.Status)
                        .Include(x=>x.AllocatedTo)
                        .Where(x=>x.Component.Id== component.Id)
                        .ToListAsync(cancellationToken: cancellationToken);

                    var auxList = new List<ActivitiesDto>();
                    if (activities is {Count: > 0} )
                    {
                         foreach (var activity in activities)
                         {
                             auxList.Add(new ActivitiesDto
                             {
                                 Description = activity.Description,
                                 Name = activity.Name,
                                 Id = activity.Id,
                                 Status = activity.Status,
                                 AllocatedTo = activity.AllocatedTo?.FullName,
                                 CreatedAt = activity.CreatedAt.ToString("yyyy-MM-dd"),
                                 StarAt = activity.StarAt.Year != 0001? activity.StarAt.ToString("yyyy-MM-dd") : "-",
                                 EndAt = activity.EndAt.Year != 00001? activity.EndAt.ToString("yyyy-MM-dd") :"-",
                                 ExpectedStarDate = activity.ExpectedStarAt.ToString("yyyy-MM-dd"),
                                 ExpectedEndDate = activity.ExpectedEndAt.ToString("yyyy-MM-dd"),
                             });
                         }

                         listToReturn.Add(new GanttActivitiesDto
                         {
                             Activities = auxList,
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
                                 FinishedActivities = activities.Count >0?  activities.Count(x => x.Status.Description == "Concluídas"): 0
                             }
                         });
                    }
                   
                }
                return listToReturn;
            }
            
            public string DateHelper(DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
        }
    }
}