using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.QueryFilters;

namespace ProyectoMascotas.Api.Responses
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Pagination Pagination { get; set; }
        public Message[] Messages { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
