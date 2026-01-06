namespace Hotel.Admin.Model;
using Hotel.User.Model;

public class Admin : User 
{
    public Admin(string username, string password) : base(username, password)
    {
        
    }
    
    public override UserRole Role => UserRole.Admin;
}