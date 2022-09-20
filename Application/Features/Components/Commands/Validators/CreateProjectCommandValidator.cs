using Application.Features.Components.Commands.RequestModel;
using FluentValidation;

namespace Application.Features.Components.Commands.Validators
{
    public class CreateProjectCommandValidator :  AbstractValidator<CreateComponentCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.DepartmentId).NotEmpty();
            RuleFor(x => x.ExpectedEndDate).NotEmpty();
            RuleFor(x => x.ExpectedStartDate).NotEmpty();
        }
    }
}