using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Hotel.Security;
using Hotel.Reservation.Model;
using Hotel.Reservation.Files;
namespace Hotel.History.ViewModel;

public class HistoryPageViewModel : INotifyPropertyChanged
{
    private RezervareFisier _fileReservation = new RezervareFisier();
    public Visibility HistoryVisibility { get; private set; }
    public Visibility notHistoryVisibility { get; private set; }
    public List<Rezervare> rezervariCustomer {get; private set;}
    public ICommand GoBackCommand { get; }
    public HistoryPageViewModel()
    {
        rezervariCustomer = new List<Rezervare>();
        List<Rezervare> rezervari = new List<Rezervare>();
        rezervari = _fileReservation.IncarcaRezervari();
        rezervariCustomer = rezervari.Where(u => (u.PersoanaRezervare.Equals(Session.CurrentUser) && u.StatusRezervare==StatusRezervare.Finalizata)).ToList();
        OnPropertyChanged(nameof(rezervariCustomer));
        if (rezervariCustomer.Count == 0)
        {
            HistoryVisibility = Visibility.Collapsed;
            notHistoryVisibility = Visibility.Visible;
        }
        else
        {
            HistoryVisibility = Visibility.Visible;
            notHistoryVisibility = Visibility.Collapsed;
        }

        GoBackCommand = new RelayCommand(ExecuteGoBack);

    }
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ExecuteGoBack()
    {
        Session.CurrentFrame.GoBack();
    }
    
}
public class RelayCommand : System.Windows.Input.ICommand
{
    private readonly Action _execute;
    public RelayCommand(Action execute) => _execute = execute;
    public bool CanExecute(object parameter) => true;
    public void Execute(object parameter) => _execute();
    public event EventHandler CanExecuteChanged;
}