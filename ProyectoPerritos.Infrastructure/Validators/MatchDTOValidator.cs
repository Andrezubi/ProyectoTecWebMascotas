using FluentValidation;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Validators
{
    public class MatchDTOValidator:AbstractValidator<MatchDTO>
    {
        public MatchDTOValidator() 
        {
            RuleFor(m => m.LostPetId)
            .NotEmpty().WithMessage("Debe existir un ID de mascota perdida.");

            RuleFor(m => m.FoundPetId)
                .NotEmpty().WithMessage("Debe existir un ID de mascota encontrada.");

            RuleFor(m => m.MatchScore)
                .InclusiveBetween(0, 1).WithMessage("El puntaje de coincidencia debe estar entre 0 y 1.");

            RuleFor(m => m.Status)
                .NotEmpty().WithMessage("El estado del match es obligatorio.")
                .MaximumLength(20);
        }
    }
}
