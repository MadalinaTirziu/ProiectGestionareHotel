
using Hotel.Users.Models;
using System.Windows;
using System.Windows.Controls;
using Hotel.SignIn;
using Hotel.Security;
using Hotel.SignUp;
using Hotel.History;
namespace Hotel;

public partial class StartUpPage : Page
{
    private static StartUpPageViewModel _vm = new StartUpPageViewModel();

    public StartUpPage()
    {
        InitializeComponent();      
        this.DataContext = _vm;
        SignInButton.Click -= SignInButton_Click;
        SignInButton.Click += SignInButton_Click;
        SignUpButton.Click -= SignUpButton_Click;
        SignUpButton.Click += SignUpButton_Click;
        HistoryButton.Click -= HistoryButton_Click;
        HistoryButton.Click += HistoryButton_Click;
        LogOutButton.Click -= LogOutButton_Click;
        LogOutButton.Click += LogOutButton_Click;
    }

    private void SignInButton_Click(object sender, RoutedEventArgs e)
    {
        SignInPage signInPage = new SignInPage();
        signInPage.LoginSucceeded += SignInPage_LoginSucceeded;

        Session.CurrentFrame.Navigate(signInPage);
    }

    private void SignUpButton_Click(object sender, RoutedEventArgs e)
    {
        SignUpPage signUpPage = new SignUpPage();
        signUpPage.SignUpSucceeded += SignUpPage_SignUpSucceeded;
        Session.CurrentFrame.Navigate(signUpPage);
    }

    private void HistoryButton_Click(object sender, RoutedEventArgs e)
    {
        HistoryPage historyPage = new HistoryPage();
        Session.CurrentFrame.Navigate(historyPage);
    }

    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
        Session.Logout();
        
    }
    private void SignInPage_LoginSucceeded(object sender, EventArgs e)
    {
        
        Session.CurrentFrame.GoBack();
    }

    private void SignUpPage_SignUpSucceeded(object sender, EventArgs e)
    {
        
        Session.CurrentFrame.GoBack();
    }

    
}