using Hotel.Admins.Model;
using Hotel.Customers.Model;
using System.Text.Json.Serialization;
namespace Hotel.Users.Models;
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Role")]
[JsonDerivedType(typeof(Admin), "admin")]
[JsonDerivedType(typeof(Customer), "customer")]


public abstract class User
{
    public string Username{get;}
    public string Password{get; private set; }

    protected User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    
    public abstract UserRole Role { get; }
}

public enum UserRole
{
    Admin,
    Customer
}