using Application.Features.ApplicationRolePermissions.Commands.RequestModels;
using FluentValidation;

namespace Application.Features.ApplicationRolePermissions.Commands.Validators
{
    public class CreateRolePermissionCommandValidator : AbstractValidator<CreateRolePermissionCommand>
    {
        public CreateRolePermissionCommandValidator()
        {
            RuleFor(x => x.ApplicationPermissionId).NotEmpty();
            RuleFor(x => x.ApplicationRoleId).NotEmpty();
        }
    }
    
}