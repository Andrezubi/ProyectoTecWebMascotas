using FluentValidation;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Validators
{
    public class LostPetDTOValidator:AbstractValidator<LostPetDTO>
    {
        public LostPetDTOValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("Debe asociar el reporte a un usuario existente.");

            RuleFor(p => p.Species)
                .NotEmpty().WithMessage("Debe indicar la especie (Perro, Gato, etc.).");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Debe ingresar el nombre de la mascota.");

            RuleFor(p => p.Color)
                .NotEmpty().WithMessage("Debe especificar el color.");

            RuleFor(p => p.Breed)
                .NotEmpty().WithMessage("Debe especificar la raza.");

            RuleFor(p => p.Sex)
                .NotEmpty().WithMessage("Debe especificar el sexo de la mascota.")
                .Must(s => s == "Macho" || s == "Hembra").WithMessage("El sexo debe ser 'Macho' o 'Hembra'.");

            RuleFor(p => p.MicroChip)
                .MaximumLength(50);

            RuleFor(p => p.Description)
                .MaximumLength(255);

            RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(p => p.DateLost)
                .NotEmpty().WithMessage("Debe indicar la fecha en que se perdió la mascota.");

            RuleFor(p => p.RewardAmount)
                .GreaterThanOrEqualTo(0).WithMessage("La recompensa no puede ser negativa.");
        }
    }
}
