using Application.Features.Users.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.Users.Commands.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Fullname).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
            
        }
    }
}