using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.QueryFilters
{
    /// <summary>
    /// Entidad La cual filtra el tamaño y la hoja de la paginacion
    /// </summary>
    public abstract class PaginationQueryFilter
    {
        //Cantidad de registros por pagina
        
        /// <summary>
        /// Cantidad de registros por pagina
        /// </summary>
        [SwaggerSchema("Cantidad de registros por pagina")]
        public int PageSize { get; set; }
        //Numero de pagina a mostrar

        /// <summary>
        /// Numero de pagina a mostrar
        /// </summary>
        [SwaggerSchema("Numero de pagina a mostrar")]
        public int PageNumber { get; set; }
    }

}
