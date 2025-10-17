using System;
using System.Collections.Generic;

namespace ProyectoMascotas.Api.Data;

public class LostPetDTO
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Species { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public string? Breed { get; set; }

    public string? Sex { get; set; }

    public string? MicroChip { get; set; }

    public string? Description { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? DateLost { get; set; }

    public string? Status { get; set; }


    public decimal RewardAmount { get; set; }

}
