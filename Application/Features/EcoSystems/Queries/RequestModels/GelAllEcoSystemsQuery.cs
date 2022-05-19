using System.Collections.Generic;
using Domain;
using MediatR;

namespace Application.Features.EcoSystems.Queries.RequestModels
{
    public class GelAllEcoSystemsQuery : IRequest<IReadOnlyList<EcoSystem>>
    {
        
    }
}