using Hotel.Users.Models;

using System.Text.Json.Serialization;

namespace Hotel.Customers.Model;

public class Customer : User 
{
    public Customer(string username, string password) : base(username, password)
    {
        
    }
    [JsonIgnore]
    public override UserRole Role => UserRole.Customer;
}