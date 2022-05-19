using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Commands.RequestModels
{
    public class CreateEcosystemCommand : IRequest<EcoSystem>
    {
        public string Description { get; set; }
        public string Url { get; set; }   
    }
}