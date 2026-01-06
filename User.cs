using System.IO;
using System.Text.Json;
namespace ProiectPoo;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Role")]
[JsonDerivedType(typeof(Admin), "admin")]
[JsonDerivedType(typeof(Customer), "customer")]


abstract class User
{
    public string Username{get;}
    public string Password{get; private set}

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