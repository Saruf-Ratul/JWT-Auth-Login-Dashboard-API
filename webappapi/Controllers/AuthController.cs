// AuthController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using webappapi.Models;
using Microsoft.AspNetCore.Cors;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.Username);
        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = registerModel.Username,
            Email = registerModel.Email,
            // Add any additional properties to set during registration
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);

        if (result.Succeeded)
        {
            // Optionally sign in the user after registration
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Return a token or other relevant information
            // For simplicity, you can generate a token here or redirect to login
            // Adjust this based on your authentication strategy
            return Ok(new { Message = "Registration successful", UserId = user.Id });
        }

        return BadRequest(new { Errors = result.Errors });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
