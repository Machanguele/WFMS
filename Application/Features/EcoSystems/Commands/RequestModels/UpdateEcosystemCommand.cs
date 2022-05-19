using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Commands.RequestModels
{
    public class UpdateEcosystemCommand : IRequest<EcoSystem>
    {
        public int EcosystemId { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
    }
}