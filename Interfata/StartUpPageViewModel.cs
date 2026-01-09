  
using System.Collections.ObjectModel;
using Hotel.Users.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Hotel.SignIn;
using Hotel.Security;
using Hotel.Room.Files;
using Hotel.Room.Model;
namespace Hotel;

public partial class StartUpPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    public StartUpPageViewModel()
    {
        Session.SessionChanged += Refresh;
        IncarcaCamere();

    }

    private void IncarcaCamere()
    {
        var data = _fisierCamera.IncarcaCamereDisponibile();
        CamereLibere = new ObservableCollection<Camera>(data);
    }
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
    
    private Visibility _UserMenuVisibility = Visibility.Collapsed;

    public Visibility UserMenuVisibility
    {
        get => _UserMenuVisibility;
        set { _UserMenuVisibility = value; OnPropertyChanged(); }
    }
    private Visibility _AdminMenuVisibility = Visibility.Collapsed;
    public Visibility AdminMenuVisibility
    {
        get => _AdminMenuVisibility;
        set { _AdminMenuVisibility = value; OnPropertyChanged(); }
    }
    private Visibility _CommonVisibility = Visibility.Visible;

    public Visibility CommonVisibility
    {
        get => _CommonVisibility;
        set { _CommonVisibility = value; OnPropertyChanged(); }
    }
    public string CurrentUsername 
    {
        get => Security.Session.CurrentUser?.Username ?? "";
        set { }
    }

    public void Refresh()
    {
        if (Session.IsAuthenticated)
        {
            SignInVisibility = Visibility.Collapsed;
            SignUpVisibility = Visibility.Collapsed;
            UserTextBoxVisibility = Visibility.Visible;
            CommonVisibility = Visibility.Visible;
            if (Session.CurrentUser.Role == UserRole.Admin)
            {
                AdminMenuVisibility = Visibility.Visible;
            }
            else
            {
                UserMenuVisibility = Visibility.Visible;
            }
        }
        else
        {
            SignInVisibility = Visibility.Visible;
            SignUpVisibility = Visibility.Visible;
            UserTextBoxVisibility = Visibility.Collapsed;
            UserMenuVisibility = Visibility.Collapsed;
            AdminMenuVisibility = Visibility.Collapsed;
            CommonVisibility = Visibility.Collapsed;
        }

        IncarcaCamere();
        OnPropertyChanged(nameof(CurrentUsername));
    }

    private CameraFisier _fisierCamera = new CameraFisier();
    private ObservableCollection<Camera> _camereLibere;
    public ObservableCollection<Camera> CamereLibere
    {
        get => _camereLibere;
        set 
        { 
            _camereLibere = value; 
            OnPropertyChanged(); 
        }
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
