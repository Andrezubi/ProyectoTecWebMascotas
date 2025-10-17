using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public partial class FoundPet
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Species { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Breed { get; set; }

    public string? Sex { get; set; }

    public string? PhotoUrl { get; set; }

    public string? MicroChip { get; set; }

    public string? Description { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? DateFound { get; set; }

    public string? Status { get; set; }


    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<PetPhoto> PetPhotos { get; set; } = new List<PetPhoto>();

    public virtual User? User { get; set; }
}
