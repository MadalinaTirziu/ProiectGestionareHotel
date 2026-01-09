using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Hotel.Customers.Model;
using Hotel.Reservation.Model;
using Hotel.Security;
using Hotel.EditSelectedReservation;
using Hotel.Reservation.Admin;
using Hotel.SignIn.ViewModel;
using Hotel.Users.Models;

namespace Hotel.EditSelectedReservation.ViewModel;

public class EditSelectedReservationPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private Rezervare _rezervareSelectata;
    public Rezervare RezervareSelectata { get => _rezervareSelectata; }
    public EditSelectedReservationPageViewModel(Rezervare rezervare)
    {
        _rezervareSelectata = rezervare;
        _ziSosire = rezervare.DataSosire.Day;
        _lunaSosire = rezervare.DataSosire.Month;
        _anSosire = rezervare.DataSosire.Year;
        _ziPlecare = rezervare.DataPlecare.Day;
        _lunaPlecare = rezervare.DataPlecare.Month;
        _anPlecare = rezervare.DataPlecare.Year;
        _numarPersoane = rezervare.NumarPersoane;
        GoBackCommand = new RelayCommand(ExecuteGoBack);
        SubmitEditCommand = new RelayCommand(ExecuteEditCommand);
    }
    private int _ziSosire;
    public int ZiSosire { get => _ziSosire; set { _ziSosire = value; OnPropertyChanged(nameof(ZiSosire)); } }

    private int _lunaSosire;
    public int LunaSosire { get => _lunaSosire; set { _lunaSosire = value; OnPropertyChanged(nameof(LunaSosire)); } }

    private int _anSosire;
    public int AnSosire { get => _anSosire; set { _anSosire = value; OnPropertyChanged(nameof(AnSosire)); } }
    
    private int _ziPlecare;
    public int ZiPlecare { get => _ziPlecare; set { _ziPlecare = value; OnPropertyChanged(nameof(ZiPlecare)); } }

    private int _lunaPlecare;
    public int LunaPlecare { get => _lunaPlecare; set { _lunaPlecare = value; OnPropertyChanged(nameof(LunaPlecare)); } }

    private int _anPlecare;
    public int AnPlecare { get => _anPlecare; set { _anPlecare = value; OnPropertyChanged(nameof(AnPlecare)); } }
    private int _numarPersoane;
    public int NumarPersoane { get => _numarPersoane; set { _numarPersoane = value; OnPropertyChanged(nameof(NumarPersoane)); } }
    
    public ICommand GoBackCommand { get; }
    public ICommand SubmitEditCommand { get; }
    private void ExecuteGoBack(object parameter)
    {
        Session.CurrentFrame.GoBack();
    }

    private void ExecuteEditCommand(object parameter)
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
                    Rezervare rezervareEditata = new Rezervare(_rezervareSelectata.CameraRezervata,Session.CurrentUser as Customers.Model.Customer,dataSosire, dataPlecare, NumarPersoane);
                    AdministrareRezervari ar = new AdministrareRezervari();
                    ar.StergereRezervare(RezervareSelectata);
                    ar.AdaugaRezervare(rezervareEditata,true);
                    Session.ChangeOccurred();
                    Session.CurrentFrame.GoBack();
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
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute)
    {
        _execute = execute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}