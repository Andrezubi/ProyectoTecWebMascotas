using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.QueryFilters
{
    /// <summary>
    /// Entidad de Filtros que se pueden aplicar a la busqueda de Usuarios
    /// </summary>
    public class UserQueryFilter:PaginationQueryFilter
    {
        /// <summary>
        /// Nombre del Usuario
        /// </summary>
        [SwaggerSchema("Id del usuario")]
        public string? Name { get; set; }

    }
}
