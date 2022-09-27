using FluentValidation;
using MVC.Data.Entities;

namespace MVC.Validation
{
    public class GateValidator : AbstractValidator<Gate>
    {
        public GateValidator()
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("The name is required");//.WithMessage("Der Name darf nicht leer sein.");
        }
    }
}
