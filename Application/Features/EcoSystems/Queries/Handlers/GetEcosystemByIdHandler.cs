using System.Threading;
using System.Threading.Tasks;
using Application.Features.EcoSystems.Queries.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Queries.Handlers
{
    public class GetEcosystemByIdHandler : IRequestHandler<GetEcoSystemByIdQuery, EcoSystem>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEcosystemByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  async Task<EcoSystem> Handle(GetEcoSystemByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<EcoSystem>().GetByIdAsync(request.EcoSystemId);
        }
    }
}