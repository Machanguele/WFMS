using Application.Features.EcoSystems.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.EcoSystems.Commands.Validators
{
    public class CreateEcosystemCommandValidator : AbstractValidator<CreateEcosystemCommand>
    {
        public CreateEcosystemCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Url).NotEmpty();
            
        }
    }
}