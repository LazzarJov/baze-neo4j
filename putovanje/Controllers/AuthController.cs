using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neo4j.Driver;
using putovanjeApp1.Dtos;
using putovanjeApp1.Models;
using putovanjeApp1.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly SymmetricSecurityKey _key;

    public AuthController(UserService userService, SymmetricSecurityKey key)
    {
        _userService = userService;
        _key = key;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var created = await _userService.RegisterAsync(user);
        return Ok(created);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var guid = await _userService.LoginAsync(request.Email, request.Password);
        if (guid == null)
            return Unauthorized("Pogrešan email ili password");

        // Kreiramo JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new { Token = jwt });
    }
}


public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
