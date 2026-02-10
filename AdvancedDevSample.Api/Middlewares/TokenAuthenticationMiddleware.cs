using Microsoft.Extensions.Configuration;

namespace AdvancedDevSample.Api.Middlewares
{
    /// <summary>
    /// Middleware d'authentification très simple par token Bearer.
    /// </summary>
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenAuthenticationMiddleware> _logger;
        private readonly string _expectedToken;

        public TokenAuthenticationMiddleware(
            RequestDelegate next,
            ILogger<TokenAuthenticationMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _expectedToken = configuration["Security:Token"] ?? string.Empty;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Laisser passer Swagger et health sans token
            if (path.StartsWith("/swagger") || path.StartsWith("/health"))
            {
                await _next(context);
                return;
            }

            if (string.IsNullOrWhiteSpace(_expectedToken))
            {
                _logger.LogWarning("Token de sécurité non configuré.");
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) ||
                !authHeader.ToString().StartsWith("Bearer ", StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Authorization Bearer token manquant." });
                return;
            }

            var providedToken = authHeader.ToString().Substring("Bearer ".Length).Trim();

            if (!string.Equals(providedToken, _expectedToken, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Token invalide." });
                return;
            }

            await _next(context);
        }
    }
}

