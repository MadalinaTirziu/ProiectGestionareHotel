using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Hotel.EditSelectedReservation;
using Hotel.EditSelectedReservation.ViewModel;
using Hotel.Reservation.Admin;
using Hotel.Security;
using Hotel.Reservation.Model;
using Hotel.Reservation.Files;

namespace Hotel.CurrentReservation.ViewModel;

public class CurrentReservationPageViewModel : INotifyPropertyChanged
{
    private RezervareFisier _fileReservation = new RezervareFisier();
    public Visibility ActiveVisibility { get; private set; }
    public Visibility notActiveVisibility { get; private set; }
    public List<Rezervare> rezervariCustomer {get; private set;}
    public ICommand GoBackCommand { get; }
    public ICommand EditReservationCommand { get; }
    public ICommand CancelCommand { get; }
    public CurrentReservationPageViewModel()
    {
        rezervariCustomer = new List<Rezervare>();
        ListaRezervari();
        if (rezervariCustomer.Count == 0)
        {
            ActiveVisibility = Visibility.Collapsed;
            notActiveVisibility = Visibility.Visible;
        }
        else
        {
            ActiveVisibility = Visibility.Visible;
            notActiveVisibility = Visibility.Collapsed;
        }

        GoBackCommand = new RelayCommand(_=>ExecuteGoBack());
        EditReservationCommand = new RelayCommand(reservation=>ExecuteEditReservation(reservation));
        CancelCommand = new RelayCommand(reservation => ExecuteCancel(reservation));
        Session.SessionChanged += Refresh;
        Refresh();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    private void ListaRezervari()
    {
        List<Rezervare> rezervari = new List<Rezervare>();
        rezervari = _fileReservation.IncarcaRezervari();
        rezervariCustomer = rezervari.Where(u => (u.PersoanaRezervare.Equals(Session.CurrentUser) && u.StatusRezervare==StatusRezervare.Activa)).ToList();
        OnPropertyChanged(nameof(rezervariCustomer));
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ExecuteGoBack()
    {
        Session.CurrentFrame.GoBack();
    }

    private void ExecuteEditReservation(object rezervare)
    {
        if (rezervare is Rezervare selected)
        {
            var editSelectedReservation = new EditSelectedReservationPage(selected);
            Session.CurrentFrame.Navigate(editSelectedReservation);
        }
    }

    private void ExecuteCancel(object reservation)
    {
        if (reservation is Rezervare selected)
        {
            AdministrareRezervari ar = new AdministrareRezervari();
            ar.ModificaStatus(selected,StatusRezervare.Anulata);
            ListaRezervari();
            Session.ChangeOccurred();
        }
    }
    void Refresh()
    {
        rezervariCustomer = new List<Rezervare>();
        ListaRezervari();
        OnPropertyChanged(nameof(rezervariCustomer));
        if (rezervariCustomer.Count == 0)
        {
            ActiveVisibility = Visibility.Collapsed;
            notActiveVisibility = Visibility.Visible;
        }
        else
        {
            ActiveVisibility = Visibility.Visible;
            notActiveVisibility = Visibility.Collapsed;
        }
        OnPropertyChanged(nameof(rezervariCustomer));
        OnPropertyChanged(nameof(ActiveVisibility));
        OnPropertyChanged(nameof(notActiveVisibility));
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