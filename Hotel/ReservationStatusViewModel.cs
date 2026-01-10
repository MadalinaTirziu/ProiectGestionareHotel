using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hotel.Admins.EditReservationStatus;
using Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using Hotel.Security;
using Hotel.SignIn.ViewModel;

namespace Hotel.Admins.ReservationStatus.ViewModel;

public class ReservationStatusViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
    private ObservableCollection<Rezervare> _rezervariActive;
    public ObservableCollection<Rezervare> RezervariActive
    {
        get { return _rezervariActive; }
        set
        {
            _rezervariActive = value;
            OnPropertyChanged(nameof(RezervariActive));
        }
    }
    private ObservableCollection<Rezervare> _rezervari;
    public ObservableCollection<Rezervare> Rezervari
    {
        get { return _rezervari; }
        set
        {
            _rezervari = value;
            OnPropertyChanged(nameof(Rezervari));
        }
    }

    private void IncarcaRezervariActive()
    {
        AdministrareRezervari _ar =  new AdministrareRezervari();
        var data =_ar.AfisareRezervari().Where(r => r.StatusRezervare == StatusRezervare.Activa).ToList();
        RezervariActive = new ObservableCollection<Rezervare>(data);
    }

    private void IncarcaRezervari()
    {
        AdministrareRezervari _ar = new AdministrareRezervari();
        var data = _ar.AfisareRezervari().ToList();
        Rezervari = new ObservableCollection<Rezervare>(data);
    }

    private Visibility _activeVisibility = Visibility.Hidden;

    public Visibility ActiveVisibility
    {
        get { return _activeVisibility; }
        set
        {
            _activeVisibility = value;
            OnPropertyChanged(nameof(ActiveVisibility));
        }
    }
    
    private Visibility _inactiveVisibility = Visibility.Hidden;

    public Visibility InactiveVisibility
    {
        get { return _inactiveVisibility; }
        set
        {
            _inactiveVisibility = value;
            OnPropertyChanged(nameof(InactiveVisibility));
        }
    }
    
    private Visibility _allVisibility = Visibility.Visible;

    public Visibility AllVisibility
    {
        get { return _allVisibility; }
        set
        {
            _allVisibility = value;
            OnPropertyChanged(nameof(AllVisibility));
        }
    }
    public ICommand SeeAllCommand { get; }
    public ICommand SeeActiveCommand { get; }
    public ICommand EditReservationCommand { get; }
    public ReservationStatusViewModel()
    {
        IncarcaRezervari();
        IncarcaRezervariActive();
        SeeAllCommand = new RelayCommand(SeeAll);
        SeeActiveCommand = new RelayCommand(SeeActive);
        EditReservationCommand = new RelayCommand(res=>EditReservation(res));
        Session.SessionChanged += Refresh;
    }

    private void SeeAll(object obj)
    {
        if (Rezervari.Count == 0)
        {
            InactiveVisibility = Visibility.Visible;
            AllVisibility = Visibility.Hidden;
            ActiveVisibility = Visibility.Hidden;
        }
        else
        {
            InactiveVisibility = Visibility.Hidden;
            AllVisibility = Visibility.Visible;
            ActiveVisibility = Visibility.Hidden;
        }
    }

    private void SeeActive(object obj)
    {
        if (RezervariActive.Count == 0)
        {
            InactiveVisibility = Visibility.Visible;
            AllVisibility = Visibility.Hidden;
            ActiveVisibility = Visibility.Hidden;
        }
        else
        {
            InactiveVisibility = Visibility.Hidden;
            AllVisibility = Visibility.Hidden;
            ActiveVisibility = Visibility.Visible;
        }
    }

   
    /*public List<StatusRezervare> StatusOptions { get; } = Enum.GetValues(typeof(StatusRezervare))
        .Cast<StatusRezervare>()
        .ToList();
    private StatusRezervare _selectedStatus;
    public StatusRezervare SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(nameof(SelectedStatus));
        }
    }*/
    private void EditReservation(object obj)
    {
        if (obj is Rezervare rezervare)
        {
            AdminEditReservationStatusPage adminEditReservationStatusPage =
                new AdminEditReservationStatusPage(rezervare);
            Session.CurrentFrame.Navigate(adminEditReservationStatusPage);
        }
    }

    

    private void Refresh()
    {
        IncarcaRezervari();
        IncarcaRezervariActive();
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
    
    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}