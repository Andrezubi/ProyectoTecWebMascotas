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
    internal class MatchConfiguration:IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Matches__3214EC078A87C254");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.FoundPet).WithMany(p => p.Matches)
                .HasForeignKey(d => d.FoundPetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Matches_FoundPet");

            builder.HasOne(d => d.LostPet).WithMany(p => p.Matches)
                .HasForeignKey(d => d.LostPetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Matches_LostPet");
        }

    }
}
