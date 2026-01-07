using Hotel.Users.Models;
using System.Windows.Controls;
namespace Hotel.Security;

public static class Session
{
    public static User? CurrentUser { get; private set; }
    public static bool IsAuthenticated => CurrentUser != null;
    public static Frame CurrentFrame { get; set; }
    public static event Action SessionChanged;

    public static void Login(User user)
    {
        CurrentUser = user;
        SessionChanged?.Invoke();
    }

    public static void Logout()
    {
        CurrentUser = null;
        
        SessionChanged?.Invoke();
    }
}