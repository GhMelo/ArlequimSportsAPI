using Domain.Entity;
using Domain.Interfaces.IRepository;
using System.Diagnostics;
using System.Text.Json;

namespace ArlequimSportsAPI.Middlewares
{
    public class GlobalExceptionAndLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionAndLoggingMiddleware> _logger;

        public GlobalExceptionAndLoggingMiddleware(RequestDelegate next, ILogger<GlobalExceptionAndLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IMongoRepository<LogRequest> logRepository)
        {
            var sw = Stopwatch.StartNew();
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            string errorMessage = string.Empty;
            int statusCode = 200;

            try
            {
                await _next(context);
                statusCode = context.Response.StatusCode;

                if (statusCode >= 400)
                {
                    errorMessage = await ExtrairMensagemErroDoCorpo(responseBody, statusCode);

                    context.Response.Body.SetLength(0);
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        statusCode,
                        message = errorMessage
                    };

                    var errorJson = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(errorJson);
                }
            }
            catch (Exception ex)
            {
                statusCode = ex switch
                {
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                errorMessage = ex.Message;

                _logger.LogError(ex, "Erro não tratado");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var errorResponse = new
                {
                    statusCode,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };

                var errorJson = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(errorJson);
            }

            sw.Stop();

            var log = new LogRequest
            {
                CorrelationId = correlationId,
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow,
                ExecutionTimeMs = sw.ElapsedMilliseconds,
                ErrorMessage = !string.IsNullOrEmpty(errorMessage) ? Truncate(errorMessage, 1000) : string.Empty
            };

            try
            {
                await logRepository.InsertOneAsync(log);
            }
            catch
            {
                // Evita quebra por falha de log
            }

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }

        private async Task<string> ExtrairMensagemErroDoCorpo(Stream responseBody, int statusCode)
        {
            if (statusCode == 401)
                return "Não autorizado";

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            try
            {
                var json = JsonDocument.Parse(responseText);
                if (json.RootElement.TryGetProperty("errors", out var errorsElement))
                {
                    var messages = new List<string>();
                    foreach (var property in errorsElement.EnumerateObject())
                    {
                        var field = property.Name;
                        foreach (var message in property.Value.EnumerateArray())
                        {
                            messages.Add($"{field}: {message.GetString()}");
                        }
                    }
                    return string.Join(" | ", messages);
                }

                return responseText;
            }
            catch
            {
                return responseText;
            }
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
