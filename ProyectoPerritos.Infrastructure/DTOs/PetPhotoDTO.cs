using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public class PetPhotoDTO
{
    public int Id { get; set; }

    public int? LostPetId { get; set; }

    public int? FoundPetId { get; set; }

    public string? PhotoUrl { get; set; }
}

