using System.Windows.Controls;
using WpfApp2.ViewModel;

namespace WpfApp2;

public partial class SignUpPage : Page
{
    public SignUpPage()
    {
        InitializeComponent();
        DataContext = new SignUpPageViewModel();
    }
}
