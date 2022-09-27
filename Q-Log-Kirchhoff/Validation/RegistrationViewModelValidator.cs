using FluentValidation;
using Microsoft.Extensions.Localization;
using MVC.Models;
using System.Globalization;
using System.Threading;

namespace MVC.Validation
{
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator(IStringLocalizer<RegistrationViewModelValidator> localizer)
        {
            RuleFor(model => model.LicensePlate).NotEmpty().WithMessage(localizer["LicensePlateRequired"]);
            RuleFor(model => model.Firstname).NotEmpty().WithMessage(localizer["FirstnameRequired"]);
            RuleFor(model => model.Lastname).NotEmpty().WithMessage(localizer["LastnameRequired"]);
            RuleFor(model => model.Forwarder).NotEmpty().WithMessage(localizer["ForwarderRequired"]);
            RuleFor(model => model.Customer).NotEmpty().WithMessage(localizer["CustomerRequired"]);
            RuleFor(model => model.Phonenumber).NotEmpty().WithMessage(localizer["MobilenumberRequired"]);
          //  RuleFor(model => model.GoodsReceiptCustomerEmpties).NotEmpty().WithMessage(localizer["ZielRequired"]);
            //RuleFor(model => model.LoadingStation).NotEmpty().WithMessage(localizer["LoadingStationRequired"]);

        }
    }
}
