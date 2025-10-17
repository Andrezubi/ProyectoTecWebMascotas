using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public partial class User
{
    public int Id { get; set; }

    public int? Ci { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? PhotoUrl { get; set; }


    public virtual ICollection<FoundPet> FoundPets { get; set; } = new List<FoundPet>();

    public virtual ICollection<LostPet> LostPets { get; set; } = new List<LostPet>();
}
