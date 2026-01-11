using System.Windows;
using System.Windows.Controls;
using Hotel.Security;
using Hotel.SignUp.ViewModel;
namespace Hotel.SignUp;

public partial class SignUpPage : Page
{
    public event EventHandler SignUpSucceeded; 

    public SignUpPage()
    {
        InitializeComponent();
        SignUpPageViewModel _vm = new SignUpPageViewModel();
        DataContext = _vm;
        GoBackButton.Click += ExecuteGoBack;
        
        _vm.SignUpSucceeded += (s, e) => SignUpSucceeded?.Invoke(this, EventArgs.Empty);
    }

    private void ExecuteGoBack(object sender, RoutedEventArgs e)
    {
        Session.CurrentFrame.GoBack();
    }
}