using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProyectoMascotas.Api.Data;

public partial class MascotasContext : DbContext
{
    public MascotasContext()
    {
    }

    public MascotasContext(DbContextOptions<MascotasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FoundPet> FoundPets { get; set; }

    public virtual DbSet<LostPet> LostPets { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<PetPhoto> PetPhotos { get; set; }

    public virtual DbSet<Security> Securities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


}
