using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Custom_Entities
{
    public class Posts
    {
        public IEnumerable<FoundPet> FoundPets { get; set; }
        public IEnumerable<LostPet> LostPets { get; set; } 
    }
}
