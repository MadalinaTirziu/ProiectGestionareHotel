using System.Windows;
using System.Windows.Controls;
using Hotel.Security;
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
        GoBackButton.Click += ExecuteGoBack;
        
        vm.LoginSucceeded += (s, e) =>
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty); 
        };
    }
    private void ExecuteGoBack(object sender, RoutedEventArgs e)
    {
        Session.CurrentFrame.GoBack();
    }
    
}
