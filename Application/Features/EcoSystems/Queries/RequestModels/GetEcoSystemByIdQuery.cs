using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Queries.RequestModels
{
    public class GetEcoSystemByIdQuery : IRequest<EcoSystem>
    {
        public GetEcoSystemByIdQuery(int ecoSystemId)
        {
            ecoSystemId = ecoSystemId;
        }
        public int EcoSystemId { get; set; }
    }
}