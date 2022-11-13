using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Users
{
    public class ListUsers
    {
        public class ListUsersQuery : IRequest<List<UserDto>>
        {
            
        }
        
        public class ListUserQueryHandler : IRequestHandler<ListUsersQuery, List<UserDto>>
        {
            private readonly DataContext _context;

            public ListUserQueryHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<UserDto>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _context.AppUsers
                    .Include(x=>x.ApplicationRole)
                    .Include(x=>x.Department)
                    .ToListAsync(cancellationToken);

                return users.Select(user => new UserDto 
                    {FullName = user.FullName, 
                    Email = user.Email, 
                    Archived = user.Archived,
                    Username = user.UserName,
                    Role = user.ApplicationRole?.Description, 
                    Department = user.Department?.ShortName})
                    .ToList();
            }
        }
    }
}