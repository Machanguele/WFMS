using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ApplicationRolePermissions.Commands.RequestModels;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.ApplicationRolePermissions.Commands.Handlers
{
    public class CreateRolePermissionHandler : IRequestHandler<CreateRolePermissionCommand, ApplicationRolePermission>
    {
        private readonly DataContext _context;

        public CreateRolePermissionHandler(DataContext context)
        {
            _context = context;
        }
        public  async Task<ApplicationRolePermission> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == request.ApplicationRoleId);
            if (role == null) throw new WebException("Role Not Found", (WebExceptionStatus) HttpStatusCode.NotFound);
          
            var permission = await _context.ApplicationPermissions.FirstOrDefaultAsync(x => x.Id == request.ApplicationPermissionId);
            if (permission == null) throw new WebException("Permission Not Found", (WebExceptionStatus) HttpStatusCode.NotFound);

            var rolePermission = new ApplicationRolePermission
            {
                ApplicationPermission = permission,
                ApplicationRole = role

            };
            await _context.AddAsync(rolePermission);
            if (await  _context.SaveChangesAsync(cancellationToken) == 0)
                throw new WebException("Fail to add role permission", (WebExceptionStatus) HttpStatusCode.NotFound);

            return rolePermission;

        }
    }
}