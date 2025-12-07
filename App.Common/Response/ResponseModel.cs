namespace App.Common.Response
{
    public interface IResponseModel
    {
        int StatusCode { get; set; }
        DateTime Timestamp { get; set; }
        bool IsError { get; set; }
        string Message { get; set; }
        object? Result { get; set; }
    }

    public class ResponseModel : IResponseModel
    {
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsError { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }

        public static ResponseModel Response(int statusCode, bool isError, string message, object? result)
        {
            return new ResponseModel
            {
                Timestamp = DateTime.UtcNow,
                StatusCode = statusCode,
                IsError = isError,
                Message = message,
                Result = result
            };
        }
        public static ResponseModel ErrorValidation(string? message, string[] errors, int code = 400)
          => Response(code, true, message ?? "Validation failed.", errors);
        public static ResponseModel Success(string message, object? data = null)
            => Response(200, false, message, data);

        public static ResponseModel Error(string message, int statusCode = 500)
            => Response(statusCode, true, message, null);
    }

    // ===== Generic =====
    public class ResponseModel<T> : IResponseModel
    {
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsError { get; set; }
        public string Message { get; set; } = string.Empty;

        public T? Result { get; set; }

        object? IResponseModel.Result
        {
            get => Result;
            set => Result = (T?)value;
        }

        public static ResponseModel<T> Response(int statusCode, bool isError, string message, T? data = default)
        {
            return new ResponseModel<T>
            {
                Timestamp = DateTime.UtcNow,
                StatusCode = statusCode,
                IsError = isError,
                Message = message,
                Result = data
            };
        }

        public static ResponseModel<T> Success(string message, T? data = default)
            => Response(200, false, message, data);

        public static ResponseModel<T> Error(string message, int statusCode = 500)
            => Response(statusCode, true, message, default);
    }
}
