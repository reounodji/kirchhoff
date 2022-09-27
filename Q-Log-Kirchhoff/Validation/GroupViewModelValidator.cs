using FluentValidation;
using MVC.Models.ConfigurationViewModels;

namespace MVC.Validation
{
    public class GroupViewModelValidator : AbstractValidator<GroupViewModel>
    {
        public GroupViewModelValidator()
        {
            RuleFor(model => model.Group.Name).NotEmpty().WithMessage("The name is required");//.WithMessage("Der Name darf nicht leer sein");
        }
    }
}
