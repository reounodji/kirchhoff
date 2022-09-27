using FluentValidation;
using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Validation
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("The name is required");//.WithMessage("Der Name darf nicht leer sein.");
        }
    }
}
