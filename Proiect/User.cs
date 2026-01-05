namespace ProiectPoo;

public enum UserRole
{
    Admin,
    Customer
}

public abstract class User
{
    public string Username{get;}
    private string Password{get;}

    protected User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public bool CheckPassword(string input)
    {
        return Password == input;
    }
    
    public abstract UserRole Role { get; }
}