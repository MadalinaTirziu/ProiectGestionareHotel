using Hotel.Users.Model;
namespace Hotel.Customers.Model;

public class Customer : User 
{
    public Customer(string username, string password) : base(username, password)
    {
        
    }
    
    public override UserRole Role => UserRole.Customer;
}