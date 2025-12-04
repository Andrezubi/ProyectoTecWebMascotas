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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-TRFH6CQT\\SQLEXPRESS;Database=DbMascotas;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
