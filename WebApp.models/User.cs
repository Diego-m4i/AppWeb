using Microsoft.AspNetCore.Identity;
namespace WebApp.models;

public class User : IdentityUser
{
    public Cart Cart { get; set; }
    public ICollection<Order> Orders { get; set; }
}
