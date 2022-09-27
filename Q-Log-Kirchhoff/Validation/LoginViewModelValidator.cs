using FluentValidation;
using MVC.Models;

namespace MVC.Validation
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().WithMessage("The username is required");//.WithMessage("Es muss ein Benutzername angegeben werden");
            RuleFor(model => model.Password).NotEmpty().WithMessage("The password is required");//.WithMessage("Es muss ein Passwort angegeben werden");
        }
    }
}
