using FluentValidation;
using MVC.Data.Entities;

namespace MVC.Validation
{
    public class DisplayConfigurationValidator : AbstractValidator<DisplayConfiguration>
    {
        public DisplayConfigurationValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("The name is required");//.WithMessage("Der Name darf nicht leer sein.");
            RuleFor(m => m.IPAddress).NotEmpty().WithMessage("The IP is required");//.WithMessage("Die IP Adresse darf nicht leer sein.");
            RuleFor(m => m.Port).NotEmpty().WithMessage("The port is required");//.WithMessage("Der Port muss angegeben werden.");
            RuleFor(m => m.Rows).NotEmpty().WithMessage("The amount of rows is required");//.WithMessage("Die Anzal der Zeilen muss angegeben werden.");
            RuleFor(m => m.CharsPerLine).NotEmpty().WithMessage("The amount of characters per row is required");//.WithMessage("Die Anzahl der Zeichen pro Zeile muss angegeben werden.");
        }
    }
}
