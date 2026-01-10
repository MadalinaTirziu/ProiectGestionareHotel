  
using System.Collections.ObjectModel;
using Hotel.Users.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hotel.CurrentReservation.ViewModel;
using Hotel.Reservation.Admin;
using Hotel.Room.Admin;
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

    private string _numarCamera;

    public string NumarCamera
    {
        get => _numarCamera;
        set { 
            _numarCamera = value; OnPropertyChanged();
            IncarcaCamere();
        }
    }
    public StartUpPageViewModel()
    {
        Session.SessionChanged += Refresh;
        IncarcaCamere();
        SetSearchDates = new RelayCommand(ExecuteDatesVisibility);
        SetSearchNumber = new RelayCommand(ExecuteNumberVisibility);
    }
    public ICommand SetSearchDates { get; set; }
    public ICommand SetSearchNumber { get; set; }
    private Visibility _DatesVisibility  = Visibility.Visible;
    public Visibility DatesVisibility
    {
        get => _DatesVisibility;
        set { _DatesVisibility = value; OnPropertyChanged(); }
    }
    private Visibility _NumberVisibility = Visibility.Collapsed ;

    public Visibility NumberVisibility
    {
        get => _NumberVisibility;
        set { _NumberVisibility = value; OnPropertyChanged(); }
    }

    private void ExecuteDatesVisibility(object parameter)
    {
        DatesVisibility = Visibility.Visible;
        NumberVisibility = Visibility.Collapsed;
    }

    private void ExecuteNumberVisibility(object parameter)
    {
        DatesVisibility = Visibility.Collapsed;
        NumberVisibility = Visibility.Visible;
    }
    private void IncarcaCamere()
    {
        if (NumarCamera is null)
        {
            
            var data = _fisierCamera.IncarcaCamereDisponibile().OrderBy(c => c.Numar).ToList();
            CamereLibere = new ObservableCollection<Camera>(data);
        }
        else
        {
            var data = _fisierCamera.IncarcaCamereDisponibile().Where(u => u.Numar.ToString().Contains(NumarCamera)).OrderBy(c => c.Numar).ToList();;
            CamereLibere = new ObservableCollection<Camera>(data);
        }
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
    private Visibility _CommonVisibility = Visibility.Collapsed;

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
    private int? _ziSosire;
    public int? ZiSosire 
    { 
        get => _ziSosire; 
        set 
        { 
            if (value == 0) _ziSosire = null;
            else _ziSosire = value;
            OnPropertyChanged(); 
            CheckToLoadList();
        } 
    }
    
    private int? _lunaSosire;
    public int? LunaSosire 
    { 
        get => _lunaSosire; 
        set 
        { 
            if (value == 0) _lunaSosire = null;
            else _lunaSosire = value;
            OnPropertyChanged(); 
            CheckToLoadList();
        } 
    }

    private int? _anSosire;
    public int? AnSosire 
    { 
        get => _anSosire; 
        set 
        { 
            if (value == 0) _anSosire = null;
            else _anSosire = value;
            OnPropertyChanged(); 
            CheckToLoadList();
        } 
    }
    
    private int? _ziPlecare;
    public int? ZiPlecare 
    { 
        get => _ziPlecare; 
        set 
        { 
            if (value == 0) _ziPlecare = null;
            else _ziPlecare = value;
            OnPropertyChanged(); 
            CheckToLoadList();
        } 
    }

    private int? _lunaPlecare;
    public int? LunaPlecare 
    { 
        get => _lunaPlecare; 
        set 
        { 
            if (value == 0) _lunaPlecare = null;
            else _lunaPlecare = value;
            OnPropertyChanged(); 
            CheckToLoadList();
        } 
    }

    private int? _anPlecare;
    public int? AnPlecare 
    { 
        get => _anPlecare; 
        set 
        { 
            if (value == 0) _anPlecare = null;
            else _anPlecare = value;
            OnPropertyChanged();
            CheckToLoadList();
        } 
    }

    private void CheckToLoadList()
    {
        try
        {


            if (ZiSosire != null && LunaSosire != null && AnSosire != null && ZiPlecare != null &&
                LunaPlecare != null &&
                AnPlecare != null)
            {

                DateOnly sosire = new DateOnly(AnSosire.Value, LunaSosire.Value, ZiSosire.Value);
                DateOnly plecare = new DateOnly(AnPlecare.Value, LunaPlecare.Value, ZiPlecare.Value);
                if (plecare <= sosire) return;
                AdministrareRezervari ar = new AdministrareRezervari();
                
                var toateCamerele = _fisierCamera.IncarcaCamere(); 
                
                var disponibile = toateCamerele
                    .Where(camera => ar.VerificaDisponibilitate(camera.Numar, sosire, plecare))
                    .ToList();

                CamereLibere = new ObservableCollection<Camera>(disponibile);
                //OnPropertyChanged(nameof(CamereLibere));
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }
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
