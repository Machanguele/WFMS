using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Activities
{
    public class ListSumQuantities
    {
        public class ListSumQuantitiesQuery : IRequest<List<SumActivitiesDto>>
        {
            
        }
        public class ListSumQuantitiesCommandHandler: IRequestHandler<ListSumQuantitiesQuery, List<SumActivitiesDto>>
        {
            private readonly DataContext _context;

            public ListSumQuantitiesCommandHandler(DataContext context)
            {
                _context = context;
            }
            public  async Task<List<SumActivitiesDto>> Handle(ListSumQuantitiesQuery request, CancellationToken cancellationToken)
            {
                var statuses = await _context.ActivityStatuses.ToListAsync(cancellationToken);
                var listToReturn = new List<SumActivitiesDto>();
                foreach (var item in statuses)
                {
                    var activities = await _context.Activities
                        .Include(x => x.Status)
                        .CountAsync(x => x.Status.Id == item.Id, cancellationToken);
                    string color = "";
                    if (item.Name == "Por fazer")
                        color = "#F39C12";
                    if (item.Name == "Em andamento")
                        color = "#3498DB";
                    if (item.Name == "Em Revisão")
                        color = "#2ECC71";
                    if (item.Name == "Concluídas")
                        color = "#27AE60";
                    
                    listToReturn.Add(new SumActivitiesDto
                    {
                        Color = color,
                        Name = item.Name,
                        Quantity = activities
                    });
                }

                return listToReturn;
            }
        }
    }
}