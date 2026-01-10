using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using Hotel.Security;
using Hotel.SignIn.ViewModel;

namespace Hotel.Admins.EditReservationStatus.ViewModel;

public class AdminEditReservationStatusViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    private Rezervare _selectedRezervare;

    public Rezervare SelectedRezervare
    {
        get => _selectedRezervare;
        set
        {
            _selectedRezervare = value;
            OnPropertyChanged(nameof(SelectedRezervare));
        }
    }
    public List<StatusRezervare> StatusOptions { get; } = Enum.GetValues(typeof(StatusRezervare))
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
    }
    public ICommand SubmitCommand { get; }
    public AdminEditReservationStatusViewModel(Rezervare rezervare)
    {
        SelectedRezervare = rezervare;
        SelectedStatus = SelectedRezervare.StatusRezervare;
        SubmitCommand = new RelayCommand(Submit);
    }

    private void Submit(object obj)
    {
        AdministrareRezervari _ar=new AdministrareRezervari();
        _ar.ModificaStatus(SelectedRezervare, SelectedStatus);
        Session.ChangeOccurred();
        Session.CurrentFrame.GoBack();
    }
}