using Application.Features.UsersSystem.commands.RequestModels;
using FluentValidation;

namespace Application.Features.UsersSystem.commands.Validators
{
    public class CreateUsersSystemsCommandValidator : AbstractValidator<CreateUsersSystemsCommand>
    {
        public CreateUsersSystemsCommandValidator()
        {
            RuleFor(x => x.UserEmail).NotEmpty();
            RuleFor(x => x.EcoSystemId).NotEmpty();
        }
        
    }
}