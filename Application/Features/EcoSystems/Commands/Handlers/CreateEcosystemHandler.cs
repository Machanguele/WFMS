using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.EcoSystems.Commands.RequestModels;
using Application.Interfaces;
using Application.Specifications;
using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Commands.Handlers
{
    public class CreateEcosystemHandler : IRequestHandler<CreateEcosystemCommand, EcoSystem>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEcosystemHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<EcoSystem> Handle(CreateEcosystemCommand request, CancellationToken cancellationToken)
        {
            var spec = new EcosystemSpecification(request.Url, request.Description);
            var ecoSystem  = await _unitOfWork.Repository<EcoSystem>().GetEntityWithSpec(spec);

            if (ecoSystem != null)
                throw new WebException($"System with URL : {request.Url} or" +
                                       $" Description {request.Description} is/are already " +
                                       $"exist(s) in database", (WebExceptionStatus) HttpStatusCode.NotFound);
            
            ecoSystem = new EcoSystem
            {
                Description = request.Description,
                Url = request.Url
            };
            
            
            _unitOfWork.Repository<EcoSystem>().AddAsync(ecoSystem);
            if (await _unitOfWork.Complete() > 0)
                return ecoSystem;
            throw new WebException($"Fail to create user", (WebExceptionStatus) HttpStatusCode.NotFound);

        }
    }
}