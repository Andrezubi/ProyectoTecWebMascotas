using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.QueryFilters
{
    internal class LostPetQueryFilter
    {
        public int? UserId { get; set; }

        public string? Species { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public string? Breed { get; set; }

        public string? Sex { get; set; }

        public DateTime? DateLost { get; set; }

        public string? MicroChip { get; set; }

        public string? Description { get; set; }
    }
}
