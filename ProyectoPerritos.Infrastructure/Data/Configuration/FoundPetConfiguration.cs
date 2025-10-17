using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Data.Configuration
{
    internal class FoundPetConfiguration:IEntityTypeConfiguration<FoundPet>
    {
        public void Configure(EntityTypeBuilder<FoundPet> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__FoundPet__3214EC07C5B38ECC");

            builder.ToTable("FoundPet");

            builder.HasIndex(e => e.MicroChip, "UQ__FoundPet__FD135BA44CBC7E53").IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Breed)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.DateFound).HasColumnType("datetime");
            builder.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            builder.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            builder.Property(e => e.MicroChip)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.PhotoUrl).HasMaxLength(255);
            builder.Property(e => e.Sex)
                .HasMaxLength(20)
                .IsUnicode(false);
            builder.Property(e => e.Species)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);


            builder.HasOne(d => d.User).WithMany(p => p.FoundPets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_FoundPet_User");
        }
        public FoundPetConfiguration()
        {
            
        }
    }
}
