using FluentValidation;
using MVC.Models.ConfigurationViewModels;

namespace MVC.Validation
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().WithMessage("The username is required");//.WithMessage("Der Benutzername darf nicht leer sein.");
        }
    }
}
