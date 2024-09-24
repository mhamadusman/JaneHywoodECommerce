using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthController(IConfiguration config)
    {
        // Ensure configuration values are loaded from appsettings.json
        _key = config["Jwt:Key"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Key is not configured.");
        _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Issuer is not configured.");
        _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Audience is not configured.");
    }

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] UserCredentials credentials)
    {
        // Simple validation for demonstration purposes
        if (credentials.Username == "test" && credentials.Password == "password")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, credentials.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        return Unauthorized();
    }
}

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
}
