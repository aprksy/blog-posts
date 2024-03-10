using System.Text.Json;

namespace api.Application.Adapters
{
    public class ApiResponse<T> : IResult
    {
        private int _statusCode = 200;

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ApiResponse(int statusCode, bool success, string message, T data)
        {
            _statusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; set; }

        public string Message { get; set; }
        
        public T Data { get; set; }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = _statusCode;

            var response = new
            {
                Success,
                Message,
                Data
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}