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


        private readonly EmailService _emailService;

        public AuthController(CakeCraftDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
     
            _emailService = emailService;
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


//forget password 


[HttpPost("forget_password")]

public async Task<IActionResult> ForgetPassword(forgotModel forgotModel)
{

    if(forgotModel == null){
        return BadRequest("Invalid or missing email");
    }

    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotModel.Email);

  

    if(existingUser == null){
        return NotFound("User not found");
    }



       // Generate reset token (random string for simplicity)
    string resetToken = Guid.NewGuid().ToString();

    existingUser.ResetToken = resetToken;
    existingUser.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

    await _context.SaveChangesAsync();


    // Send email (implement EmailService to send the token)
    string resetLink = $"https://localhost:7134/reset-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(existingUser.Email)}";
    await _emailService.SendEmailAsync(forgotModel.Email, "Password Reset", $"Click the link to reset your password: {resetLink}");


    return Ok(new { message = "Password reset link has been sent to your email.", resetToken });
   
}



//reset password

[HttpPost("reset_password")]

public async Task<IActionResult> ResetPassword([FromBody] resetPasswordModel resetPasswordModel)
{
    if(resetPasswordModel == null){
        return BadRequest("Invalid or missing reset data");
    }

    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordModel.Email);

    if(existingUser == null){
        return NotFound("User not found");
    }

    if(existingUser.ResetToken != resetPasswordModel.Token){
        return BadRequest("Invalid reset token");
    }

    if(existingUser.ResetTokenExpiry < DateTime.UtcNow){
        return BadRequest("Reset token has expired");
    }

    var passwordHasher = new PasswordHasher<User>();
    existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, resetPasswordModel.Password);

    existingUser.ResetToken = null;
    existingUser.ResetTokenExpiry = null;

    await _context.SaveChangesAsync();

    return Ok("Password reset successful");
}
    }
}
