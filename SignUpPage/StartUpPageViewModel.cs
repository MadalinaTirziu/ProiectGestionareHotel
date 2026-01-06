  
using Hotel.Users.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Hotel.SignIn;
using Hotel.Security;

namespace Hotel;

public partial class StartUpPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    
    private Visibility _signInVisibility = Visibility.Visible;
    public Visibility SignInVisibility 
    { 
        get => _signInVisibility; 
        set { _signInVisibility = value; OnPropertyChanged(); } 
    }

    private Visibility _signUpVisibility = Visibility.Visible;
    public Visibility SignUpVisibility 
    { 
        get => _signUpVisibility; 
        set { _signUpVisibility = value; OnPropertyChanged(); } 
    }

    private Visibility _userTextBoxVisibility = Visibility.Collapsed;
    public Visibility UserTextBoxVisibility 
    { 
        get => _userTextBoxVisibility; 
        set { _userTextBoxVisibility = value; OnPropertyChanged(); } 
    }
    
    
    public string CurrentUsername 
    {
        get => Security.Session.CurrentUser?.Username ?? "";
        set { /* Do nothing, it's read-only logic-wise */ }
    }

    public void Refresh()
    {
        if (Security.Session.IsAuthenticated)
        {
            SignInVisibility = Visibility.Collapsed;
            SignUpVisibility = Visibility.Collapsed;
            UserTextBoxVisibility = Visibility.Visible;
        }
        else
        {
            SignInVisibility = Visibility.Visible;
            SignUpVisibility = Visibility.Visible;
            UserTextBoxVisibility = Visibility.Collapsed;
        }
        
        OnPropertyChanged(nameof(CurrentUsername));
    }
    private void SignInButton_Click(object sender, RoutedEventArgs e)
    {
        
        SignInPage signInPage = new SignInPage();

        
        signInPage.LoginSucceeded += SignInPage_LoginSucceeded;
       
        ((MainWindow)Application.Current.MainWindow).MainPage.Navigate(signInPage);
    }

    private void SignUpButton_Click(object sender, RoutedEventArgs e)
    {
        //SignInPage signInPage = new SignInPage();

        
        //signInPage.LoginSucceeded += SignInPage_LoginSucceeded;
       
        //((MainWindow)Application.Current.MainWindow).MainPage.Navigate(signInPage);
    }
    private void SignInPage_LoginSucceeded(object sender, EventArgs e)
    {
        
        Frame mainFrame = ((MainWindow)Application.Current.MainWindow).MainPage;
        if (mainFrame.CanGoBack)
            mainFrame.GoBack();
        else
            MessageBox.Show("Login successful!");
    }
}
