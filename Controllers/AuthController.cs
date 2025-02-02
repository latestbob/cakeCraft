using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CakeCraftApi.Models;
using CakeCraftApi.Data;
using Microsoft.EntityFrameworkCore;

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

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }
    // public async Task<IActionResult> Register(User user)
    // {
    //     if (user == null)
    //     {
    //         return BadRequest("Invalid user data.");
    //     }

    //     // Check if user already exists
    //     var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
    //     if (existingUser != null)
    //     {
    //         return Conflict("User already exists.");
    //     }

    //     // Add user to the database
    //     _context.Users.Add(user);
    //     await _context.SaveChangesAsync();

    //     return Ok("User registered successfully.");
    // }


    }
}
