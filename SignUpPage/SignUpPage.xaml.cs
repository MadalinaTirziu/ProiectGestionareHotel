using System.Windows.Controls;
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
        
        
        _vm.SignUpSucceeded += (s, e) => SignUpSucceeded?.Invoke(this, EventArgs.Empty);
    }
}