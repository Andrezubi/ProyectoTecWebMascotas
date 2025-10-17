using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public partial class PetPhoto
{
    public int Id { get; set; }

    public int? LostPetId { get; set; }

    public int? FoundPetId { get; set; }

    public string? PhotoUrl { get; set; }

    public virtual FoundPet? FoundPet { get; set; }

    public virtual LostPet? LostPet { get; set; }
}
