using FluentValidation;
using SolicitaFacil.Shared.DTOs.UserDTOs;

namespace SolicitaFacil.Shared.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$");
        RuleFor(x => x.Role).NotEmpty();
    }
}

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$");
        RuleFor(x => x.Role).NotEmpty();
    }
}