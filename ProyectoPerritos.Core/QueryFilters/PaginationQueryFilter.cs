using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.QueryFilters
{
    public abstract class PaginationQueryFilter
    {
        //Cantidad de registros por pagina
        public int PageSize { get; set; }
        //Numero de pagina a mostrar
        public int PageNumber { get; set; }
    }

}
