using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Hotel.Customers.Model;
using Hotel.History.ViewModel;
using Hotel.Reservation.Admin;
using Hotel.Reservation.Files;
using Hotel.Reservation.Model;
using Hotel.Room.Model;
using Hotel.Security;
using Hotel.Users.Models;

namespace Hotel.RoomPage.ViewModel;

public class CameraPageViewModel :INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler CamereChanged;
    public ICommand GoBackCommand { get; }   
    public ICommand SubmitCommand { get; }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private Camera _room;
    public Camera SelectedRoom
    {
        get => _room;
        set { _room = value; OnPropertyChanged(nameof(SelectedRoom)); }
    }
    private Rezervare _rezervare;

    public Rezervare CurrentRezervare
    {
        get => _rezervare;
        set { _rezervare = value; OnPropertyChanged(nameof(CurrentRezervare));}
    }
    // --- Arrival (Sosire) ---
    private int _ziSosire;
    public int ZiSosire { get => _ziSosire; set { _ziSosire = value; OnPropertyChanged(nameof(ZiSosire)); } }

    private int _lunaSosire;
    public int LunaSosire { get => _lunaSosire; set { _lunaSosire = value; OnPropertyChanged(nameof(LunaSosire)); } }

    private int _anSosire;
    public int AnSosire { get => _anSosire; set { _anSosire = value; OnPropertyChanged(nameof(AnSosire)); } }

    // --- Departure (Plecare) ---
    private int _ziPlecare;
    public int ZiPlecare { get => _ziPlecare; set { _ziPlecare = value; OnPropertyChanged(nameof(ZiPlecare)); } }

    private int _lunaPlecare;
    public int LunaPlecare { get => _lunaPlecare; set { _lunaPlecare = value; OnPropertyChanged(nameof(LunaPlecare)); } }

    private int _anPlecare;
    public int AnPlecare { get => _anPlecare; set { _anPlecare = value; OnPropertyChanged(nameof(AnPlecare)); } }
    private int _numarPersoane;
    public int NumarPersoane { get => _numarPersoane; set { _numarPersoane = value; OnPropertyChanged(nameof(NumarPersoane)); } }
    public CameraPageViewModel(Camera room)
    {
       SelectedRoom = room;
       GoBackCommand = new RelayCommand(ExecuteGoBack);
       SubmitCommand = new RelayCommand(Submit);
    }
    
    private void ExecuteGoBack()
    {
        Session.CurrentFrame.GoBack();
    }
    private void Submit()
    {
        try
        {
            if (Session.IsAuthenticated)
            {
                if (AnPlecare < AnSosire || ZiPlecare < ZiSosire || LunaPlecare < LunaSosire)
                {
                    MessageBox.Show("Date invalide!");
                }
                else
                {
                    DateOnly dataSosire = new DateOnly(AnSosire, LunaSosire, ZiSosire);
                    DateOnly dataPlecare = new DateOnly(AnPlecare, LunaPlecare, ZiPlecare);
                    AdministrareRezervari rf = new AdministrareRezervari();
                    if (Session.CurrentUser.Role == UserRole.Customer && rf.VerificaDisponibilitate(SelectedRoom.Numar,dataSosire, dataPlecare))
                    {
                        Customer DateCustomer = new Customer(Session.CurrentUser.Username, Session.CurrentUser.Password);
                        CurrentRezervare = new Rezervare(SelectedRoom, DateCustomer, dataSosire, dataPlecare,
                            NumarPersoane);
                        MessageBox.Show(
                            $"Ati trimis rezervarea cu succes!\n {CurrentRezervare.DataSosire}\n {CurrentRezervare.DataPlecare}\n {CurrentRezervare.NumarPersoane}\n");
                        rf.AdaugaRezervare(CurrentRezervare,false);
                        Session.ChangeOccurred();
                        Session.CurrentFrame.GoBack();
                    }
                    else
                    {
                        if(Session.CurrentUser.Role != UserRole.Customer) MessageBox.Show($"Nu sunteti logat intr-un cont de Customer");
                        else if(rf.VerificaDisponibilitate(SelectedRoom.Numar,dataSosire, dataPlecare) == false) MessageBox.Show($"Camera e rezervata in acea perioada");
                    }
                }
            }
            else
                MessageBox.Show("Logati-va inainte de a face rezervarea");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}