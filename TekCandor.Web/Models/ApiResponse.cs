namespace TekCandor.Web.Models
{
    public class ApiResponse<T>
    {
        public required string Status { get; set; } // success | error
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Status = "success",
                Data = data,
                StatusCode = statusCode,
                ErrorMessage = null
            };
        }

        public static ApiResponse<T> Error(string errorMessage, int statusCode = 500)
        {
            return new ApiResponse<T>
            {
                Status = "error",
                Data = default,
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
