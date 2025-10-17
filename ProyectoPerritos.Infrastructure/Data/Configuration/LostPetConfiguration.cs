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
    internal class LostPetConfiguration: IEntityTypeConfiguration<LostPet>
    {
        public void Configure(EntityTypeBuilder<LostPet> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__LostPet__3214EC076C1C2293");

            builder.ToTable("LostPet");

            builder.HasIndex(e => e.MicroChip, "UQ__LostPet__FD135BA4FB37DCF4").IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Breed)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.DateLost).HasColumnType("datetime");
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
            builder.Property(e => e.RewardAmount).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.Sex)
                .HasMaxLength(20)
                .IsUnicode(false);
            builder.Property(e => e.Species)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.User).WithMany(p => p.LostPets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_LostPet_User");
        }

    }
}
