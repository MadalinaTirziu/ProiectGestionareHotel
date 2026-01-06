using System.Windows.Controls;
using Hotel.SignIn.ViewModel;

namespace Hotel.SignIn;

public partial class SignInPage : Page
{
    public event EventHandler LoginSucceeded; 

    public SignInPage()
    {
        InitializeComponent();

        var vm = new SignInPageViewModel();
        DataContext = vm;

        
        vm.LoginSucceeded += (s, e) =>
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty); 
        };
    }
    
}
