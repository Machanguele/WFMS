using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Queries.RequestModel
{
    public class LisAllComponentsGroupByDpt
    {
        public class LisAllComponentsGroupByDptQuery : IRequest<List<ComponentsGroupDeptDto>>
        {
            
        }
        public class LisAllComponentsGroupByDptQueryHandler : IRequestHandler<LisAllComponentsGroupByDptQuery, List<ComponentsGroupDeptDto>>
        {
            private readonly DataContext _context;

            public LisAllComponentsGroupByDptQueryHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<ComponentsGroupDeptDto>> Handle(LisAllComponentsGroupByDptQuery request, CancellationToken cancellationToken)
            {
                var components = await _context.Components
                    .Include(x => x.Department)
                    .ToListAsync();
                
                var departmants = await _context.Departments
                    .ToListAsync();

                var listToReturn = new List<ComponentsGroupDeptDto>();
                
                foreach (var dept in departmants)
                {
                    var componentsAux = components.ToList()
                        .Where(x => x.Department.Id == dept.Id);
                    listToReturn.Add(new ComponentsGroupDeptDto()
                    {
                        Department = dept,
                        Components = componentsAux
                    });
                }
                return listToReturn;
            }
        }
    }
}