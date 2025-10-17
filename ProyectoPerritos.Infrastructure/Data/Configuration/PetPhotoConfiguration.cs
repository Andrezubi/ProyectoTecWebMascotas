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
    internal class PetPhotoConfiguration:IEntityTypeConfiguration<PetPhoto>
    {
        public void Configure(EntityTypeBuilder<PetPhoto> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__PetPhoto__3214EC072EA46F51");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.PhotoUrl).HasMaxLength(255);

            builder.HasOne(d => d.FoundPet).WithMany(p => p.PetPhotos)
                .HasForeignKey(d => d.FoundPetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PetPhotos_FoundPet");

            builder.HasOne(d => d.LostPet).WithMany(p => p.PetPhotos)
                .HasForeignKey(d => d.LostPetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PetPhotos_LostPet");
        }

    }
}
