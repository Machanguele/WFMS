using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Errors;
using Application.Features.Users.Commands.RequestModels;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Users.Commands.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public CreateUserHandler(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public  async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Name == request.Role);
            if (role == null)
                throw new WebException("Role not found", (WebExceptionStatus) HttpStatusCode.NotFound);
 
            var department = await _context.Departments
                .FirstOrDefaultAsync(x => x.ShortName == request.Department, cancellationToken: cancellationToken);
            if (department == null)
                throw new WebException("Department not found", (WebExceptionStatus) HttpStatusCode.NotFound);

            var user = new AppUser
            {
                Email = request.Email,
                FullName = request.Fullname,
                EmailConfirmed = true,
                ApplicationRole = role,
                UserName = request.Email,
                Department = department
            };

            var result = await _userManager.CreateAsync(user, "Pa$$w0rd");
            if (!result.Succeeded)
                throw new WebException("User not created", (WebExceptionStatus) HttpStatusCode.BadRequest);
            return new UserDto
            {
                Department = user.Department?.ShortName,
                Email = user.FullName,
                Username = user.UserName,
                FullName = user.FullName,
                ApplicationRole = new ApplicationRole
                {
                    Description = user.ApplicationRole.Description,
                    Name = user.ApplicationRole.Name,
                    Id = user.ApplicationRole.Id,
                }
            };
        }
    }
}