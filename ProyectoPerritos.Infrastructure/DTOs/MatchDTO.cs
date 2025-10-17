using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public class MatchDTO
{
    public int Id { get; set; }

    public int? LostPetId { get; set; }

    public int? FoundPetId { get; set; }

    public double? MatchScore { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

}
