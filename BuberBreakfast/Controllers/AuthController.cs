using BuberBreakfast.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {

  // Service for CRUD operations for User
  private readonly UserManager<ApplicationUser> _userManager;
  // Service for sign-in and sign-out
  private readonly SignInManager<ApplicationUser> _signInManager;
  // To allow access to configuration settings
  private readonly IConfiguration _configuration;

  public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
  {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterObject registerObject) {
    var user = new ApplicationUser { UserName = registerObject.Email, Email = registerObject.Email };
    var result = await _userManager.CreateAsync(user, registerObject.Password);

    if (result.Succeeded)
    {
        return Ok(new { Message = "User registered successfully" });
    }

    return BadRequest(result.Errors);
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginObject loginObject)
  {
    var result = await _signInManager.PasswordSignInAsync(loginObject.Email, loginObject.Password, false, false);

    if (result.Succeeded)
    {
        var user = await _userManager.FindByNameAsync(loginObject.Email);
        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
    return Unauthorized();
  }

  private string GenerateJwtToken(ApplicationUser user) {
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Issuer"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}