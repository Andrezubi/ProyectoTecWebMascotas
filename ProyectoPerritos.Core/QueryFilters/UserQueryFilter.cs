using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.QueryFilters
{
    public class UserQueryFilter:PaginationQueryFilter
    {
        public string? Name { get; set; }

    }
}
