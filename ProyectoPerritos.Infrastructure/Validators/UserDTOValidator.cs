using FluentValidation;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Validators
{
    public class UserDTOValidator:AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(u => u.Id)
            .NotEmpty().WithMessage("El Id es obligatorio")
            .GreaterThan(0).WithMessage("El Id no puede ser negativo");


            RuleFor(u => u.Ci)
            .NotEmpty().WithMessage("El CI es obligatorio.")
            .GreaterThan(0).WithMessage("El CI debe ser mayor que 0.");

            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Debe ingresar un correo válido.")
                .MaximumLength(50).WithMessage("El correo no puede superar los 50 caracteres");

            RuleFor(u => u.Phone)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(@"^\d{7,15}$").WithMessage("El teléfono debe tener entre 7 y 15 dígitos.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");

            RuleFor(u => u.PhotoUrl)
                .MaximumLength(255)
                .When(u => !string.IsNullOrEmpty(u.PhotoUrl))
                .WithMessage("La URL de la foto no debe exceder los 255 caracteres.");
        }
    }

}

