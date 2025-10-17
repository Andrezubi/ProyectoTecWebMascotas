using FluentValidation;
using ProyectoMascotas.Core.Custom_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Validators
{
    public class GetByIdRequestValidator:AbstractValidator<GetByIdRequest>
    {
        public GetByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("El ID es requerido")
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0")
                .LessThanOrEqualTo(10000000).WithMessage("El ID no puede ser mayor a 10,000,000")
                .Must(BeAValidIdFormat).WithMessage("El ID debe ser un número válido");
        }

        private bool BeAValidIdFormat(int id)
        {
            // Validaciones adicionales para el formato del ID
            return id.ToString().Length <= 7; // Máximo 7 dígitos
        }
    }
}
