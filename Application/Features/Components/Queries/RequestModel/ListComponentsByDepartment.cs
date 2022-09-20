using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Components.Queries.RequestModel
{
    public class ListComponentsByDepartment
    {
        public class ListComponentsByDepartmentQuery : IRequest<List<Component>>
        {
            public ListComponentsByDepartmentQuery(int departmentId)
            {
                DepartmentId = departmentId;
            }
            public int  DepartmentId { get; set; }
        }
        public class ListAllComponentsQueryHandler : IRequestHandler<ListComponentsByDepartmentQuery, List<Component>>
        {
            private readonly DataContext _context;

            public ListAllComponentsQueryHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Component>> Handle(ListComponentsByDepartmentQuery request, CancellationToken cancellationToken)
            {
                return await _context.Components
                    .Include(x => x.Department)
                    .Where(x=>x.Department.Id == request.DepartmentId)
                    .ToListAsync();
            }
        }
    }
}