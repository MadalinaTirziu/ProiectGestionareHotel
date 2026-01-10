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

    public override bool Equals(object? obj)
    {
        if(obj == null) return false;
        User? other = obj as User;
        if(other == null) return false;
        return this.Username == other.Username && this.Password == other.Password && this.Role == other.Role;
    }
    [JsonIgnore]
    public abstract UserRole Role { get; }
}

public enum UserRole
{
    Admin,
    Customer
}