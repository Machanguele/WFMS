using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.EcoSystems.Queries.RequestModels;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Queries.Handlers
{
    public class GetAllEcoSystemHandler : IRequestHandler<GelAllEcoSystemsQuery, IReadOnlyList<EcoSystem>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEcoSystemHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<EcoSystem>> Handle(GelAllEcoSystemsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<EcoSystem>().GetAllAsync();
        }
    }
}