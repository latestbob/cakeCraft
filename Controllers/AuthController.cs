using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CakeCraftApi.Models;
using CakeCraftApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CakeCraftApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Xml.Schema;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IConfiguration _configuration;
        private readonly CakeCraftDbContext _context;

        public AuthController(CakeCraftDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


    //register a user
    [HttpPost("register")]

    public async Task<IActionResult> Register(User user)
    {
        if(user == null){
            return BadRequest("Invalid user data.");
        }
        
        //check if user exist

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if(existingUser != null){
            return Conflict("User already exists");
        }

        //Add user to databasd

        //hash password

        var passwordHasher = new PasswordHasher<User>();

        // user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);



        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }
    

    //login a user


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    
    {
        if (loginModel == null)
        {
            return BadRequest("Invalid login data.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

        if (existingUser == null)
        {
            return BadRequest("Invalid email or password.");
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, loginModel.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return BadRequest("Invalid email or password.");
        }

        var authService = new AuthService(_configuration);
        var token = authService.GenerateJwtToken(existingUser.Email, existingUser.UserType);

        return Ok(new { token });
    }




// get user details


[Authorize]
[HttpGet("user_details")]

public async Task<IActionResult> GetUserDetails()
{
    var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    if (string.IsNullOrEmpty(email))
        return Unauthorized(new { message = "Invalid token or not authorized" });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if(user == null){
            return NotFound("User not found");
        }



        user.PasswordHash = null;
        return Ok(user);

}


    }
}
