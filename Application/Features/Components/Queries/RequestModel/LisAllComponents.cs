using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Queries.RequestModel
{
    public class LisAllComponents
    {
        public class ListAllComponentsQuery : IRequest<List<ComponentDto>>
        {
            public string Role { get; set; }
            public string Email { get; set; }
            
        }
        public class ListAllComponentsQueryHandler : IRequestHandler<ListAllComponentsQuery, List<ComponentDto>>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;

            public ListAllComponentsQueryHandler(DataContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
            public async Task<List<ComponentDto>> Handle(ListAllComponentsQuery request, CancellationToken cancellationToken)
            {
                var listToreturn = new List<ComponentDto>();

                //var loggedUserEmail =  _userAccessor.GetCurrentUserEmail();
                //var loggedUser = await _context.Users.FirstOrDefaultAsync(x=>x.Email.ToLower() == loggedUserEmail.ToLower(), cancellationToken: cancellationToken);
                
                var components = await _context.Components
                    .Include(x => x.Department)
                    .Include(x => x.ComponentStatus)
                    .OrderBy(x=>x.CreatedAt)
                    .ToListAsync(cancellationToken: cancellationToken);

                foreach (var component in components)
                {
                    var activities = await _context.Activities
                        .Include(x=>x.Status)
                        .Where(x => x.Component.Id == component.Id)
                        .ToListAsync(cancellationToken: cancellationToken);
                    
                    listToreturn.Add(new ComponentDto()
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
                       Activities = activities,
                       Finished = component.Finished,
                       FinishedActivities = activities.Count >0?  activities.Count(x => x.Status.Description == "Concluídas"): 0
                    });
                }
                
                return listToreturn;
            }

            public string DateHelper(DateTime dateTime)
            {
                return dateTime.Year != 0001? dateTime.ToShortDateString() : "";
            }
        }
    }
}