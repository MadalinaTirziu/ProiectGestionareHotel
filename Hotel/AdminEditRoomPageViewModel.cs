using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Hotel.Room.Admin;
using Hotel.Room.Model;
using Hotel.Security;
using Hotel.SignIn.ViewModel;

namespace Hotel.Admins.EditRoom.ViewModel;

public class AdminEditRoomPageViewModel : INotifyPropertyChanged
{
    private Camera _selectedRoom;
    public List<StatusCamera> StatusOptions { get; } = Enum.GetValues(typeof(StatusCamera))
        .Cast<StatusCamera>()
        .ToList();
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private StatusCamera _selectedStatus;
    
    private int _numarCamera { get; set; }

    public int NumarCamera
    {
        get => _numarCamera;
        set
        {
            _numarCamera = value;
            OnPropertyChanged(nameof(NumarCamera));
        }
    }
    public StatusCamera SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(nameof(SelectedStatus));
            
        }
    }
    public Camera SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            _selectedRoom = value;
        }
    }
    public ICommand GoBackCommand { get; }
    public ICommand SubmitEditCommand { get; }
    public AdminEditRoomPageViewModel(Camera selectedRoom)
    {
        SelectedRoom = selectedRoom;
        NumarCamera = selectedRoom.Numar;
        GoBackCommand = new RelayCommand(ExecuteGoBack);
        SubmitEditCommand = new RelayCommand(ExecuteEdit);
    }

    private void ExecuteGoBack(object parameter)
    {
        Session.CurrentFrame.GoBack();
    }

    private void ExecuteEdit(object parameter)
    {
        AdministrareCamere _ac = new AdministrareCamere();
        if (SelectedRoom.Numar == NumarCamera)
        {
            _ac.SetareStatusCamera(NumarCamera,SelectedStatus);
            Session.ChangeOccurred();
            ExecuteGoBack(new object());
        }
        else
        {
            if (_ac.AfisareCamere().Where(c => c.Numar == NumarCamera).Any())
            {
                MessageBox.Show($"Eroare camera cu {NumarCamera} exista deja");
            }
            else
            {
                _ac.StergeCamera(SelectedRoom.Numar);
                _ac.AdaugaCamera(NumarCamera, SelectedStatus);
                MessageBox.Show("Adaugarea a avut loc cu succes!");
                Session.ChangeOccurred();
                ExecuteGoBack(new object());
                
            }
        }
    }
}