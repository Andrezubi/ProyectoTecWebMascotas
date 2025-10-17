using FluentValidation;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Validators
{
    public class PetPhotoDTOValidator:AbstractValidator<PetPhotoDTO>
    {
        public PetPhotoDTOValidator()
        {
            RuleFor(p => p.PhotoUrl)
                .NotEmpty().WithMessage("Debe incluir una URL de la foto.")
                .MaximumLength(255);

            RuleFor(p => p)
                .Must(p => p.LostPetId != null || p.FoundPetId != null)
                .WithMessage("La foto debe estar asociada a una mascota perdida o encontrada, no puede estar vacía en ambos campos.");
        }
    }
}
