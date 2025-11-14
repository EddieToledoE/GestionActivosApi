namespace GestionActivos.API.Middleware
{
    /// <summary>
    /// Clase de respuesta genérica para endpoints exitosos.
    /// Para errores, se usa ProblemDetails (RFC 7807) automáticamente mediante el middleware.
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la respuesta.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Datos de la respuesta.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Crea una respuesta exitosa con datos.
        /// </summary>
        /// <param name="data">Datos a retornar</param>
        /// <param name="message">Mensaje opcional</param>
        /// <returns>ApiResponse con Success = true</returns>
        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// Crea una respuesta exitosa sin datos (solo mensaje).
        /// </summary>
        /// <param name="message">Mensaje de éxito</param>
        /// <returns>ApiResponse con Success = true</returns>
        public static ApiResponse<T> SuccessResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message
            };
        }
    }

    /// <summary>
    /// Clase de respuesta sin tipo genérico para operaciones sin datos de retorno.
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Crea una respuesta exitosa sin datos.
        /// </summary>
        /// <param name="message">Mensaje de éxito</param>
        /// <returns>ApiResponse con Success = true</returns>
        public new static ApiResponse SuccessResponse(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }
    }
}

