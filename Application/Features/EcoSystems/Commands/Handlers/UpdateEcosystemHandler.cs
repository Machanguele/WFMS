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
    public class UpdateEcosystemHandler : IRequestHandler<UpdateEcosystemCommand, EcoSystem>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEcosystemHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  async Task<EcoSystem> Handle(UpdateEcosystemCommand request, CancellationToken cancellationToken)
        {
            var spec = new EcosystemSpecification(request.URL, request.Description, request.EcosystemId);
            var ecoSystem  = await _unitOfWork.Repository<EcoSystem>().GetEntityWithSpec(spec);

            if (ecoSystem != null)
                throw new WebException($"System with URL : {request.URL} or" +
                                       $" Description {request.Description} is/are already " +
                                       $"exist(s) in database", (WebExceptionStatus) HttpStatusCode.NotFound);
          
            ecoSystem = await _unitOfWork.Repository<EcoSystem>().GetByIdAsync(request.EcosystemId);
            if (ecoSystem == null)
            {
                throw new WebException($"Ecosystem not found", (WebExceptionStatus) HttpStatusCode.NotFound);
            }

            ecoSystem.Description = request.Description;
            ecoSystem.Url = request.URL;
            
            _unitOfWork.Repository<EcoSystem>().Update(ecoSystem);
            if (await _unitOfWork.Complete() > 0)
                return ecoSystem;
            throw new WebException($"Fail to edit system", (WebExceptionStatus) HttpStatusCode.NotFound);        }
    }
}