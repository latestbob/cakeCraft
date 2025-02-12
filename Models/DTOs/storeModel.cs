



namespace CakeCraftApi.Models.DTOs
{
    public class storeModel
{
   
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Logo { get; set; }

  
    public int VendorId { get; set; }
   
    
}
}