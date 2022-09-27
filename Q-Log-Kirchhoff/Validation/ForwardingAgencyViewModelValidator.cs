using FluentValidation;
using MVC.Models.ConfigurationViewModels;

namespace MVC.Validation
{
    public class ForwardingAgencyViewModelValidator : AbstractValidator<ForwardingAgencyViewModel>
    {
        public ForwardingAgencyViewModelValidator()
        {
            RuleFor(model => model.ForwardingAgency.Name).NotEmpty().WithMessage("The name is required");//.WithMessage("Der Name darf nicht leer sein.");
        }
    }
}
