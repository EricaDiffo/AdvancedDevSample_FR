using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdvancedDevSample.Api.Controllers
{
    public record AuthRequest(string Username, string Password);

    public record AuthResponse(string AccessToken, DateTime ExpiresAt);

    public class JwtSettings
    {
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public string SecretKey { get; init; } = string.Empty;
        public int TokenLifetimeMinutes { get; init; } = 60;
    }

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthController(IOptions<JwtSettings> jwtOptions, IConfiguration configuration)
        {
            _jwtSettings = jwtOptions.Value;
            _configuration = configuration;
        }

        /// <summary>
        /// Retourne un jeton JWT Bearer pour un utilisateur valide.
        /// </summary>
        /// <remarks>
        /// Pour la démo, les identifiants sont configurés dans <c>appsettings.json</c> (section <c>Auth:DemoUser</c>).
        /// </remarks>
        /// <response code="200">Jeton généré.</response>
        /// <response code="401">Identifiants invalides.</response>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AuthResponse> GetToken([FromBody] AuthRequest request)
        {
            if (!ValidateUser(request.Username, request.Password))
            {
                return Unauthorized(new { error = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_jwtSettings.TokenLifetimeMinutes);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.Username),
                new(JwtRegisteredClaimNames.Sub, request.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthResponse(encodedToken, expires));
        }

        private bool ValidateUser(string username, string password)
        {
            string demoUser = "admin";
            string demoPassword = "admin123!";

            return string.Equals(username, demoUser, StringComparison.OrdinalIgnoreCase)
                   && password == demoPassword;
        }
    }
}

