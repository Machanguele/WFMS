using Application.Features.Projects.Commands.RequestModel;
using FluentValidation;

namespace Application.Features.Projects.Commands.Validators
{
    public class CreateProjectCommandValidator :  AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            //RuleFor(x => x.DepartmentId).NotEmpty();
            RuleFor(x => x.EndAt).NotEmpty();
            RuleFor(x => x.StartAt).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty();
        }
    }
}