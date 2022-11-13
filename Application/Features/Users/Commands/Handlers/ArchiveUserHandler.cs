using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users.Commands.RequestModels;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Users.Commands.Handlers
{
    public class ArchiveUserHandler : IRequestHandler<ArchiveUserCommand, UserDto>
    {
        private readonly DataContext _context;

        public ArchiveUserHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<UserDto> Handle(ArchiveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers
                .Where(x=>x.Email.ToLower() == request.Email.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if(user == null)
                throw new WebException("Email not found", (WebExceptionStatus) HttpStatusCode.NotFound);

            if (user.Archived)
                user.Archived = false;
            else
                user.Archived = true;
                    
            _context.Entry(user).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (result>0)
            {
                return new UserDto
                {
                    Email = user.Email
                };
            }
            throw new WebException("Fail to create user", (WebExceptionStatus) HttpStatusCode.BadRequest);
        }
    }
}