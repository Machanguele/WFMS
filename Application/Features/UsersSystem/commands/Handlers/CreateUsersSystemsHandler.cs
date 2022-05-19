using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.UsersSystem.commands.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.UsersSystem.commands.Handlers
{
    public class CreateUsersSystemsHandler : IRequestHandler<CreateUsersSystemsCommand, UserSystem>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public CreateUsersSystemsHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<UserSystem> Handle(CreateUsersSystemsCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email.ToLower() == request.UserEmail.ToLower());
            if(user == null)
                throw new WebException($"User with email: {request.UserEmail} not found", (WebExceptionStatus) HttpStatusCode.NotFound);
            var ecoSystem = await _unitOfWork.Repository<EcoSystem>().GetByIdAsync(request.EcoSystemId);
            if (ecoSystem == null)
                throw new WebException($"System not found", (WebExceptionStatus) HttpStatusCode.NotFound);

            var userSystem = new UserSystem
            {
                AppUser = user,
                EcoSystem = ecoSystem,
                LastUpdate = DateTime.Now,
                LastUpdateCommittedToTargetSystem = false
            };
            
             _unitOfWork.Repository<UserSystem>().AddAsync(userSystem);
             if (await _unitOfWork.Complete() > 0)
                 return userSystem;
             throw new WebException($"Fail do allow user to system", (WebExceptionStatus) HttpStatusCode.BadRequest);
             
        }
    }
}