using FluentValidation;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoMascotas.Infrastructure.Validators
{
    public class FoundPetDTOValidator: AbstractValidator<FoundPetDTO>
    {
        public FoundPetDTOValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("Debe asociar el reporte a un usuario.");

            RuleFor(p => p.Species)
                .NotEmpty().WithMessage("Debe indicar la especie.");

            // El nombre puede ser NULL (no siempre se conoce)
            When(p => p.Name != null, () => {
                RuleFor(p => p.Name)
                    .MaximumLength(50)
                    .WithMessage("El nombre no debe exceder los 50 caracteres.");
            });

            RuleFor(p => p.Color)
                .NotEmpty().WithMessage("Debe especificar el color.");

            RuleFor(p => p.Breed)
                .NotEmpty().WithMessage("Debe especificar la raza.");

            RuleFor(p => p.Sex)
                .Must(s => s == "Macho" || s == "Hembra")
                .WithMessage("El sexo debe ser 'Macho' o 'Hembra'.");

            RuleFor(p => p.PhotoUrl)
                .MaximumLength(255);

            RuleFor(p => p.MicroChip)
                .MaximumLength(50);

            RuleFor(p => p.Description)
                .MaximumLength(255);

            RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(p => p.DateFound)
                .NotEmpty().WithMessage("Debe indicar la fecha en que se encontró la mascota.");
        }


    }
}
