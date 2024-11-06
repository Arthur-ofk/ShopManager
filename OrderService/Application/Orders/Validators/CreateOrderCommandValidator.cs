using Application.Orders.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.");
            RuleFor(x => x.ShippingAddress).NotEmpty().WithMessage("Shipment address is required.");
            RuleForEach(x => x.Items).SetValidator(new CreateOrderItemDtoValidator()); 
        }
    }
}
