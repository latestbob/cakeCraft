using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CakeCraftApi.Models;
using CakeCraftApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeCraftApi.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CakeCraftDbContext _context;

        public UsersController(IConfiguration configuration, CakeCraftDbContext context){
            
            _configuration = configuration;
            _context = context;
        }



        // user crud endpont

        [HttpGet("fetch")]

        public async Task<IActionResult> Fetchusers(){

        
        // var users = await _context.Set<User>().ToListAsync();
        // return Ok(users);

        var users = await _context.Set<User>().ToListAsync();

        foreach (var user in users)
        {
            user.PasswordHash = null;
        }

        return Ok(users);


    }

    //get unique user

    [HttpGet("{id}")]
    

    public async Task<IActionResult> GetUniqueUser(int id){

        var user = await _context.Users.FindAsync(id);

        if(user == null){
            return NotFound();
        }

        return Ok(user);
    }



    //get users by role

    [HttpGet("role/{role}")]

    public async Task<IActionResult> GetUsersByRole(int role){

        if(role > 3){
            return BadRequest("Invalid Role");
        }

        // var users = await _context.Users.Where(u => (int)u.Role == role).ToListAsync();
        // return Ok(users);

        var users = await _context.Users.Where(u => (int)u.Role == role).ToListAsync();
        
        return Ok(users);

    }

    //dete user by id

    [HttpDelete("delete{id}")]

    public async Task<IActionResult> DeleteUserById(int id)
    {
      
        var user = await _context.Users.FindAsync(id);

        if(user == null){
            return BadRequest("User not found");
        }

        //remove the user

        _context.Users.Remove(user);
        
        //save the changes

        await _context.SaveChangesAsync();

        //return success

        return Ok("User deleted succesfully");
    }

    //get user by uniqueId

    [HttpGet("unique/{uniqueId}")]

    public async Task<IActionResult> GetUserByUniqueId(Guid uniqueId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UniqueId == uniqueId);

        if(user == null){
            return BadRequest("Usern not Found");
        }
        

        return Ok(user);

    }




}
}