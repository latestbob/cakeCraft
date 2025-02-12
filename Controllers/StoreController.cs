using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CakeCraftApi.Models;
using CakeCraftApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CakeCraftApi.Models.DTOs;

namespace CakeCraftApi.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CakeCraftDbContext _context;

        public StoreController(IConfiguration configuration, CakeCraftDbContext context){
            
            _configuration = configuration;
            _context = context;
        }



  
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("fetch")]

        public async Task<IActionResult> FetchAllStore(){

        

        var stores = await _context.Stores.Include(s => s.Vendor).ToListAsync(); // Eager load the Vendor propertyToListAsync();


        return Ok(stores);


    }

    //get unique user


[Authorize(Policy = "AdminOnly")]
[HttpPost("create-store")]

    public async Task<IActionResult> CreateStore(storeModel storeModel)
    {
        if(storeModel == null){
            return BadRequest("Invalid store data.");
        }

        //check if store exist

        var existedStore = await _context.Stores.FirstOrDefaultAsync(s => s.Name == storeModel.Name);

        if(existedStore != null){
            return Conflict("Store with name already exists");
        }

        //Add store to database

        var store = new Store
    {
        Name = storeModel.Name,
        Description = storeModel.Description,
        Address = storeModel.Address,
        City = storeModel.City,
        State = storeModel.State,
        Country = storeModel.Country,
        Phone = storeModel.Phone,
        Email = storeModel.Email,
        Website = storeModel.Website,
        Logo = storeModel.Logo,
        VendorId = storeModel.VendorId


    };

    

        _context.Stores.Add(store);

        await _context.SaveChangesAsync();

        return Ok("Store created successfully");
    }


}
}