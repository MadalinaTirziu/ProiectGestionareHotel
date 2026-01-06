using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hotel.Users.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Role")]
[JsonDerivedType(typeof(Admin.Model.Admin), "admin")]
[JsonDerivedType(typeof(Customer.Model.Customer), "customer")]


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
    
    public enum UserRole
    {
        Admin,
        Customer
    }
}